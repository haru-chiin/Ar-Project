using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ActiveObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject activeObject;
    [SerializeField] private TMP_Dropdown toolsDropdown;
    [SerializeField] private TMP_Dropdown seedsDropdown;


    [Header("Tool Prefabs with Animations")]
    [SerializeField] private GameObject hoePrefab;
    [SerializeField] private GameObject wateringCanPrefab;
    [SerializeField] private GameObject seedBagPrefab;

    // 12 Prefabs untuk 12 jenis tanaman, setiap jenis tanaman memiliki 4 tahap pertumbuhan
    [Header("Plant Prefabs for Different Stages")]
    [SerializeField] private GameObject[] plantPrefabA; // Prefabs untuk Tanaman A (Bibit, Tahap 1, Tahap 2, Tahap 3)
    [SerializeField] private GameObject[] plantPrefabB; // Prefabs untuk Tanaman B
    [SerializeField] private GameObject[] plantPrefabC; // Prefabs untuk Tanaman C


    private ToolType selectedTool = ToolType.None;
    private string selectedSeed = "None";

    private enum ToolType
    {
        None,
        Hoe,
        WateringCan,
        SeedBag
    }

    private enum PlantingStage
    {
        None,
        Tilled,
        SeedPlanted,
        Watered,
        Finished
    }

    private PlantingStage currentStage = PlantingStage.None;
    private bool isSoilWet = false;
    private int wateringCount = 0; // Menyimpan jumlah iterasi penyiraman
    private GameObject currentPlantPrefab; // Untuk menyimpan prefab tanaman yang aktif

    // Dictionary untuk memetakan seed ke prefab
    private Dictionary<string, GameObject[]> plantPrefabs;

    private void Start()
    {
        toolsDropdown.onValueChanged.AddListener(OnToolChanged);
        seedsDropdown.onValueChanged.AddListener(OnSeedChanged);

        // Inisialisasi dictionary untuk mapping benih ke prefab tanaman
        plantPrefabs = new Dictionary<string, GameObject[]>
        {
            { "Might Seed", plantPrefabA },
            { "Adamant Seed", plantPrefabB },
            { "Parashroom", plantPrefabC },
        };

        // Validasi awal untuk memastikan prefabs sesuai
        ValidatePlantPrefabs();
    }

    private void OnToolChanged(int index)
    {
        selectedTool = (ToolType)index;
        Debug.Log("Selected tool: " + selectedTool);
    }

    private void OnSeedChanged(int index)
    {
        selectedSeed = seedsDropdown.options[index].text;
        Debug.Log("Selected seed: " + selectedSeed);

        // Reset tanaman ketika memilih benih yang baru
        if (selectedSeed == "None")
        {
            currentStage = PlantingStage.None;
            Destroy(currentPlantPrefab); // Hapus tanaman yang ada sebelumnya
            Debug.Log("No seed selected.");
            return; // Keluar jika "None" dipilih
        }

        // Validasi seed dengan dictionary
        if (plantPrefabs.ContainsKey(selectedSeed))
        {
            Debug.Log("Seed found in plantPrefabs dictionary.");
        }
        else
        {
            Debug.LogError("Seed not found in plantPrefabs dictionary");
        }
    }

    public void SetActiveObject(GameObject obj)
    {
        if (activeObject != obj)
        {
            activeObject = obj;
            Debug.Log("Active object set to: " + activeObject.name);
        }
        else if (activeObject == obj)
        {
            Debug.Log("Active object pressed again: " + activeObject.name);
        }

        PerformToolAction();
    }

    private void PerformToolAction()
    {
        if (activeObject == null) return;

        UVTilePosition uvTilePosition = activeObject.GetComponent<UVTilePosition>();
        if (uvTilePosition == null)
        {
            Debug.LogError("Active object does not have a UVTilePosition component.");
            return;
        }

        switch (selectedTool)
        {
            case ToolType.Hoe:
                if (currentStage == PlantingStage.None)
                {
                    Debug.Log("Using Hoe on: " + activeObject.name);
                    ActivateAnimationPrefab(hoePrefab);
                    uvTilePosition.ShiftUVY(0.5f); // Menaikkan tanaman ke status "digged"
                    currentStage = PlantingStage.Tilled;
                }
                else
                {
                    Debug.Log("You cannot till the soil at this stage.");
                }
                break;

            case ToolType.SeedBag:
                if (currentStage == PlantingStage.Tilled && selectedSeed != "None")
                {
                    // Menanam biji pada posisi tengah objek yang aktif
                    Debug.Log("Planting " + selectedSeed + " on: " + activeObject.name);
                    ActivateAnimationPrefab(seedBagPrefab);

                    // Menambahkan prefab tanaman pertama di posisi tengah objek aktif
                    Vector3 position = activeObject.transform.position; // Menggunakan posisi tengah objek aktif
                    if (plantPrefabs.ContainsKey(selectedSeed))
                    {
                        // Ambil array prefab berdasarkan benih yang dipilih
                        GameObject[] selectedPlantPrefabs = plantPrefabs[selectedSeed];
                        currentPlantPrefab = Instantiate(selectedPlantPrefabs[0], position, activeObject.transform.rotation); // Instansiasi tanaman tahap pertama
                        currentStage = PlantingStage.SeedPlanted;
                    }
                    else
                    {
                        Debug.LogError("Seed not found in plantPrefabs dictionary");
                    }
                }
                else if (selectedSeed == "None")
                {
                    Debug.Log("You must select a valid seed.");
                }
                else
                {
                    Debug.Log("You must till the soil first, and select a seed.");
                }
                break;

            case ToolType.WateringCan:
                if (currentStage == PlantingStage.SeedPlanted)
                {
                    if (!isSoilWet)
                    {
                        StartCoroutine(WetSoilCoroutine(uvTilePosition));
                    }
                    else
                    {
                        Debug.Log("Soil is already wet. Wait before watering again.");
                    }
                    ActivateAnimationPrefab(wateringCanPrefab);
                }
                else
                {
                    Debug.Log("You must plant a seed before watering.");
                }
                break;

            case ToolType.None:
                Debug.Log("No tool selected");
                return;
        }
    }

    private IEnumerator WetSoilCoroutine(UVTilePosition uvTilePosition)
    {
        isSoilWet = true;
        Debug.Log("Soil is wet!");

        uvTilePosition.ShiftUVX(0.5f);

        yield return new WaitForSeconds(5f);

        isSoilWet = false;
        uvTilePosition.ShiftUVX(-0.5f);

        wateringCount++;

        // Ambil array prefab tanaman berdasarkan benih yang dipilih
        if (plantPrefabs.ContainsKey(selectedSeed))
        {
            GameObject[] selectedPlantPrefabs = plantPrefabs[selectedSeed];

            // Ganti prefab tanaman pada setiap iterasi penyiraman
            if (wateringCount == 1)
            {
                ReplacePlantPrefab(selectedPlantPrefabs[1]); // Ganti dengan tanaman tahap kedua
            }
            else if (wateringCount == 2)
            {
                ReplacePlantPrefab(selectedPlantPrefabs[2]); // Ganti dengan tanaman tahap ketiga
            }
            else if (wateringCount >= 3)
            {
                currentStage = PlantingStage.Finished;
                ReplacePlantPrefab(selectedPlantPrefabs[3]); // Ganti dengan tanaman tahap akhir
                Debug.Log("Final watering iteration completed. Plant is fully grown.");
            }
        }
        else
        {
            Debug.LogError("Selected seed does not exist in plantPrefabs.");
        }
    }

    private void ReplacePlantPrefab(GameObject newPlantPrefab)
    {
        if (currentPlantPrefab != null)
        {
            Destroy(currentPlantPrefab); // Hapus tanaman lama jika ada
        }

        Vector3 position = activeObject.transform.position; // Mendapatkan posisi tengah objek aktif
        currentPlantPrefab = Instantiate(newPlantPrefab, position, activeObject.transform.rotation); // Instansiasi prefab baru
    }

    private void ActivateAnimationPrefab(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned for this tool!");
            return;
        }

        GameObject instantiatedPrefab = Instantiate(prefab, activeObject.transform.position, Quaternion.identity);
        instantiatedPrefab.SetActive(true);

        Animator animator = instantiatedPrefab.GetComponent<Animator>();
        if (animator != null)
        {
            float animationDuration = GetAnimationDuration(animator);
            Destroy(instantiatedPrefab, animationDuration);
        }
        else
        {
            Debug.LogError("Animator not found on prefab: " + prefab.name);
        }
    }

    private float GetAnimationDuration(Animator animator)
    {
        if (animator.runtimeAnimatorController != null)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            if (clips.Length > 0)
            {
                return clips[0].length;
            }
        }
        return 5.0f; // Default duration
    }

    public void ResetAll()
    {
        UVTilePosition uvTilePosition = activeObject.GetComponent<UVTilePosition>();

        uvTilePosition.ResetUVs();
        activeObject = null;
        selectedTool = ToolType.None;
        selectedSeed = "None";
        toolsDropdown.value = 0;
        seedsDropdown.value = 0;
        wateringCount = 0;
        currentStage = PlantingStage.None;
        Destroy(currentPlantPrefab); // Hapus tanaman yang ada saat reset
        Debug.Log("All settings have been reset.");
    }

    private void ValidatePlantPrefabs()
    {
        if (plantPrefabs == null || plantPrefabs.Count == 0)
        {
            Debug.LogError("Plant prefabs dictionary is empty! Please assign prefabs in the Inspector.");
        }
        else
        {
            foreach (var key in plantPrefabs.Keys)
            {
                Debug.Log("Loaded seed prefab for: " + key);
            }
        }
    }
}
