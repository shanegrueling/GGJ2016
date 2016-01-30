using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using itemsystem;
using System.Collections.Generic;

public class inventoryManager : MonoBehaviour {

	public Hashtable slots = new Hashtable();
	public Dictionary<string, item> shopitems = new Dictionary<string, item>();

	public float buyMultiplier = 1.0f;
	public float sellMultiplier = 0.6f;

	public GameObject inventoryScreen;
	public GameObject shopScreen;
	public GameObject karmaScreen;

	public int karma = 100;



	void Start() {

		shopitems ["water"] 		= new water();
		shopitems ["bamboosticks"] 	= new bambooSticks();
		shopitems ["demonarrows"] 	= new demonArrows();
		shopitems ["incensestick"] 	= new incenseSticks();
		shopitems ["earth"] 		= new earth();
		shopitems ["paperrunes"] 	= new paperRunes();
	}
	// END Start()



	void Update() {

		karmaScreen.transform.Find ("text").GetComponent<Text> ().text = karma.ToString ();
	}
	// END Update()
		


	/**
	 * add item to inventory
	 * 
	 * @param string item is a string of the item to add (e.g. water, bambus, etc...)
	 */
	public void addToInventory(string item) {

		// enough karma?
		if (karma < (int)shopitems [item].price) {

			return;
		}


		// add item to player inventar
		if (slots.Contains (item)) {

			slots[item] = (int)slots[item] + 1;
		} else {

			slots [item] = 1;
		}

		// substract karma costs from player karma
		karma -= (int)shopitems [item].price;
		
		// display item in inventory and update number textfield
		inventoryScreen.transform.Find ("slot_" + item).Find ("icon").gameObject.SetActive (true);
		inventoryScreen.transform.Find ("slot_" + item).Find ("icon").Find("textwrapper").Find ("num").gameObject.GetComponent<Text>().text = slots [item].ToString();
	}
	// END addToInventory()



	/**
	 * sell an item from the inventory
	 * 
	 * @param string item is a string of the item to sell (e.g. water, bambus, etc...)
	 */
	public void sellItem(string item) {

		// does the player has at least one of the given items
		if((int)slots[item] <= 0) {

			return;
		}

		int price = (int)Mathf.Floor((int)shopitems[item].price * sellMultiplier);

		// add karma to player karma
		karma += price;

		// substract item
		slots[item] = (int)slots[item] - 1;

		// sold the last one (hide it in the inventory)
		if((int)slots[item] == 0) {

			inventoryScreen.transform.Find ("slot_" + item).Find ("icon").gameObject.SetActive (false);
		}

		// display new number
		inventoryScreen.transform.Find ("slot_" + item).Find ("icon").Find("textwrapper").Find ("num").gameObject.GetComponent<Text>().text = slots [item].ToString();
	}
	// END sellItem()



	/**
	 * just remove an item from the inventory
	 * 
	 * @param string item is a string of the item to sell (e.g. water, bambus, etc...)
	 */
	public void removeItem(string item) {

		// does the player has at least one of the given items
		if((int)slots[item] <= 0) {

			return;
		}

		// substract item
		slots[item] = (int)slots[item] - 1;

		// sold the last one (hide it in the inventory)
		if((int)slots[item] == 0) {

			inventoryScreen.transform.Find ("slot_" + item).Find ("icon").gameObject.SetActive (false);
		}

		// display new number
		inventoryScreen.transform.Find ("slot_" + item).Find ("icon").Find("textwrapper").Find ("num").gameObject.GetComponent<Text>().text = slots [item].ToString();
	}
	// END removeItem()



	/**
	 * return the description of an item
	 */
	public string getDescriptionof(string itemtype) {

		return shopitems [itemtype].description;
	}
	// END getDescription()



	/**
	 * dispay an items description in the tooltip area of the shop
	 */
	public void displayItemTooltipInShop(string itemtype) {

		this.shopScreen.transform.Find("tooltip").Find("text").GetComponent<Text>().text = shopitems[itemtype].name + ": " + shopitems[itemtype].description;
		this.shopScreen.transform.Find("price").Find("text").GetComponent<Text>().text = shopitems[itemtype].price.ToString();
	}
	// END displayItemTooltipInShop()



	/**
	 * hides the tooltip text in the shop
	 */
	public void hideTooltipInShop() {

		this.shopScreen.transform.Find ("tooltip").Find ("text").GetComponent<Text> ().text = "";
		this.shopScreen.transform.Find ("price").Find ("text").GetComponent<Text> ().text = "";
	}
	// END hideTooltipInShop()
}
