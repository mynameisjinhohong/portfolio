using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{

	public Color[] colors;

	public HexGrid_Develope hexGrid;

	int tile = 0;

	HexCell ti;

	public HexCell[] cellPrefab;

	void Awake()
	{
		SelectColor(0);
	}

	void Update()
	{
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			HandleInput();
		}
	}

	void HandleInput()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit))
		{
			if(hit.transform.gameObject.tag == "Tile")
            {
				Transform t = hit.transform;
				HexCell hex = hit.transform.gameObject.GetComponent<HexCell>();
				ti = Instantiate(cellPrefab[tile]);
				ti.neighbors = hex.neighbors;
				ti.coordinates = hex.coordinates;
				ti.transform.position = t.position;
				Destroy(hit.transform.gameObject);

            }

		}
		Invoke("HexTileInput",0.001f);
		hexGrid.SaveMap();
		
	}
	void HexTileInput()
    {
		for (int i = 0; i < hexGrid.cells.Length; i++)
		{
			if (hexGrid.cells[i] == null)
			{
				hexGrid.cells[i] = ti;
			}
		}
	}

	public void SelectColor(int index)
	{
		tile = index;
	}
}