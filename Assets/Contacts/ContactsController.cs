﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Brolton.GUI;
using LudumDare39;

public class ContactsController : MonoBehaviour, ITableViewDataSource, ITableViewDelegate
{
    [SerializeField]
    TableView _tableView;
    [SerializeField]
    GameObject _contactCellPrefab;

    private List<ContactData> _contacts;

    public int CurrentContactId = -1;

	float cellHeight = 1;

    public void Init(List<ContactData> contacts)
    {
        _contacts = contacts;

        _tableView.CellPrefab = _contactCellPrefab;
        _tableView.Init(this, this);
        _tableView.GetComponent<TableViewScroll>().enabled = false;

//        _tableView.onSelectCell.AddListener (OnScrollTable);
//        _tableView.onSelectCell.AddListener (OnChooseCurrentCell);

        _tableView.ScrollToCell (_tableView.NumberOfRows () / 2);

//        LoadTablePosition(_tableView);
    }

    #region ITableViewDataSource

    public int NumberOfRowsInTableView(TableView tableView)
    {
        return _contacts.Count;
    }

    public float SizeForRowInTableView(TableView tableView, int row)
    {
		cellHeight = _tableView.GetComponent<RectTransform>().rect.size.y / NumberOfRowsInTableView(tableView);
		return cellHeight;
    }

    public TableViewCell CellForRowInTableView(TableView tableView, int row)
    {
        TableViewCell cell = tableView.ReusableCellForRow(row);
		cell.GetComponent<RectTransform>().sizeDelta = new Vector2(cell.GetComponent<RectTransform>().sizeDelta.x, cellHeight);
		cell.GetComponent<LayoutElement> ().minHeight = cellHeight;
		cell.GetComponent<LayoutElement> ().preferredHeight = cellHeight;
        ContactCell contactCell = cell.GetComponent<ContactCell>();
        //            displayItem.BaseSize = Constants.SEAL_SIZE;
        //            displayItem.IconPanelActive = false;
        //            ItemInfo itemInfo = _sealsItems[row].ItemInfo;
        //            displayItem.SetItemInfo(itemInfo);
        //              displayItem->setActive(true);
        contactCell.Init(_contacts[row]);

        return cell;
    }

    #endregion

    #region ITableViewDelegate

    public void TableViewDidHighlightCellForRow(TableView tableView, int row)
    {
        //          Log.Write("TableViewDidHighlightCellForRow : " + row);
    }

    public void TableViewDidSelectCellForRow(TableView tableView, int row)
    {
        CurrentContactId = row;
        AppController.Instance.OnContactClick(row);
//        _tableView.ScrollToCell (row, 0.5f);
        //          Log.Write("TableViewDidSelectCellForRow : " + row);

    }

    #endregion


    public void TurnOnIndicatorForContact(int contactId)
    {
        _tableView.CellForRow(contactId).GetComponent<ContactCell>().TurnOnIndicator();
    }
}
