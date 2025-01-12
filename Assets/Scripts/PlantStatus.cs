/*using UnityEngine;
using System.Collections;

public class PlantStatus : MonoBehaviour
{
    private bool isWatering = false;
    private bool isWet = false;  // Status apakah tanaman basah atau kering, dimulai dengan kering
    private bool isGrowing = false; // Status untuk apakah tanaman tumbuh atau tidak
    private UVTilePosition uvTilePosition; // Menyimpan referensi ke UVTilePosition untuk mengubah posisi UV

    // Waktu tumbuh tanaman (misalnya 5 detik per fase tumbuh)
    public float growthTime = 5f; // Setiap 5 detik kita ingin menandakan pertumbuhan tanaman
    private float growthTimer = 0f; // Waktu yang dihitung saat tanaman basah
    private float wetTimeAccumulator = 0f; // Menyimpan total waktu basah

    [SerializeField] private GameObject growthIndicatorPrefab; // Prefab objek penanda untuk pertumbuhan
    private bool growthIndicatorActive = false;

    private void Start()
    {
        uvTilePosition = GetComponent<UVTilePosition>(); // Mendapatkan referensi ke UVTilePosition
        if (uvTilePosition == null)
        {
            Debug.LogError("UVTilePosition component missing from the plant object!");
        }

        // Memulai siklus penyiraman, hanya jika tanaman disiram
        StartWateringCycle();
    }

    // Fungsi untuk mulai menyiram tanaman setiap 5 detik
    public void StartWateringCycle()
    {
        if (!isWatering)
        {
            StartCoroutine(WateringCycle());
        }
    }

    private IEnumerator WateringCycle()
    {
        isWatering = true;

        while (true)
        {
            yield return new WaitForSeconds(5f);  // Tunggu selama 5 detik

            // Cek jika tanaman tidak basah, maka statusnya kering dan waktunya tidak dihitung
            if (!isWet)
            {
                Debug.Log("Tanaman kering! Tidak ada perhitungan waktu.");
                ResetUV(); // Reset UV ketika tanaman kering
                growthTimer = 0f;  // Reset waktu pertumbuhan saat tanaman kering
            }
            else
            {
                // Jika tanaman basah, lanjutkan menghitung waktu basah
                wetTimeAccumulator += 5f; // Menambah waktu basah setiap 5 detik

                // Aktifkan penanda pertumbuhan setiap 5 detik saat basah
                if (wetTimeAccumulator >= growthTime && !growthIndicatorActive)
                {
                    ActivateGrowthIndicator();
                    growthIndicatorActive = true;
                    Debug.Log("Tanaman tumbuh setelah 5 detik basah!");
                }
                else if (wetTimeAccumulator >= growthTime * 2 && growthIndicatorActive)
                {
                    // Menumbuhkan tanaman setiap kali wetTime mencapai 10 detik, 15 detik, dll
                    growthIndicatorActive = false; // Menonaktifkan indikator agar tidak aktif terus menerus
                    growthTimer += growthTime;
                    Debug.Log("Tanaman tumbuh setelah 10 detik basah!");
                    // Anda dapat mengganti ini dengan animasi atau perubahan visual lainnya.
                }
            }

            // Tunggu sampai disiram
            yield return new WaitUntil(() => isWet);  // Menunggu sampai tanaman disiram dan basah
        }
    }

    // Fungsi untuk menyiram tanaman
    public void WaterPlant()
    {
        if (!isWet)
        {
            isWet = true; // Tanaman basah saat pertama kali disiram
            Debug.Log("Tanaman disiram!");
            ShiftUVX(0.5f);  // Geser UVX untuk menandakan tanaman basah
            ResetUV();  // Reset UV ketika tanaman disiram
            wetTimeAccumulator = 0f;  // Reset waktu basah setelah disiram
            growthIndicatorActive = false; // Menonaktifkan indikator sampai tanaman benar-benar tumbuh
        }
    }

    // Fungsi untuk menggeser posisi UV X untuk menandakan tanaman basah
    private void ShiftUVX(float xShift)
    {
        if (uvTilePosition != null)
        {
            uvTilePosition.ShiftUVX(xShift); // Geser UVX untuk menandakan bahwa tanaman disiram
        }
    }

    // Fungsi untuk reset UV
    private void ResetUV()
    {
        if (uvTilePosition != null)
        {
            uvTilePosition.ResetUVs(); // Reset UV ketika tanaman disiram atau tanaman kering
        }
    }

    // Fungsi untuk mengaktifkan penanda pertumbuhan tanaman
    private void ActivateGrowthIndicator()
    {
        if (growthIndicatorPrefab != null)
        {
            Instantiate(growthIndicatorPrefab, transform.position, Quaternion.identity);
        }
    }

    // Update pertumbuhan tanaman
    private void Update()
    {
        if (isGrowing)
        {
            // Jika tanaman sedang tumbuh, mulai menghitung waktu
            growthTimer += Time.deltaTime;

            // Setelah mencapai waktu tertentu, tanaman tumbuh
            if (growthTimer >= growthTime * 4)  // Setelah 20 detik basah (misalnya)
            {
                growthTimer = 0f;
                Debug.Log("Tanaman tumbuh setelah 20 detik basah!");
                // Lakukan sesuatu setelah tanaman tumbuh (misalnya mengubah skala, posisi, atau animasi)
            }
        }
    }
}
*/