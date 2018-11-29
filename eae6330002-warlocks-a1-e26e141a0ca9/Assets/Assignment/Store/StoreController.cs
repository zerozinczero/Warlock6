using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour {

    [SerializeField] private Transform mainAreaContent = null;
    [SerializeField] private Transform sideAreaContent = null;

    [SerializeField] private List<Purchasable> mainItems = new List<Purchasable>();
    [SerializeField] private List<Purchasable> sideItems = new List<Purchasable>();

    [SerializeField] private PlayerGameStats localPlayerStats = null;
    [SerializeField] private Unit target = null;
    [SerializeField] private Game game = null;

    [SerializeField] private Text goldAmount = null;

    private void Start() {
        MakeItemInstances();
        UpdateUi();
    }

    void MakeItemInstances() {
        List<Purchasable> mainPrefabs = new List<Purchasable>(mainItems);
        mainItems.Clear();
        foreach(Purchasable purchasablePrefab in mainPrefabs) {
            Purchasable purchasable = Instantiate<Purchasable>(purchasablePrefab);
            purchasable.Target = target;
            purchasable.Game = game;
            mainItems.Add(purchasable);
        }

        List<Purchasable> sidePrefabs = new List<Purchasable>(sideItems);
        sideItems.Clear();
        foreach (Purchasable purchasablePrefab in sidePrefabs) {
            Purchasable purchasable = Instantiate<Purchasable>(purchasablePrefab);
            purchasable.Target = target;
            purchasable.Game = game;
            sideItems.Add(purchasable);
        }
    }

    public void Purchase(IPurchasable purchasable) {
        int cost = purchasable.Cost;
        if(cost <= localPlayerStats.Gold) {
            localPlayerStats.Gold -= cost;
            purchasable.OnPurchased();
            UpdateUi();
        }
    }

    public void UpdateUi() {
        goldAmount.text = localPlayerStats.Gold.ToString();

        ClearArea(mainAreaContent);
        FillArea(mainAreaContent, mainItems);

        ClearArea(sideAreaContent);
        FillArea(sideAreaContent, sideItems);
    }

    void ClearArea(Transform areaContent) {
        for (int i = areaContent.childCount - 1; i >= 0; i--) {
            Destroy(areaContent.GetChild(i).gameObject);
        }
    }

    void FillArea(Transform areaContent, List<Purchasable> purchasables) {
        foreach(IPurchasable purchasable in purchasables) {
            PurchasableCell prefab = purchasable.CellPrefab;
            PurchasableCell cell = Instantiate<PurchasableCell>(prefab, areaContent);
            cell.SetPurchasable(purchasable);
            cell.OnPurchase.AddListener(Purchase);
        }
    }

    public void Close() {
        gameObject.SetActive(false);
    }

}
