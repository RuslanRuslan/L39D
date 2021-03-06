﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Brolton.GUI;

namespace LudumDare39
{
    public class ChatController : MonoBehaviour, ITableViewDataSource, ITableViewDelegate
    {
        [SerializeField]
        TableView _tableView;
        [SerializeField]
        GameObject _msgCellPrefab;

        [SerializeField]
        AnswerController _answerController;

        private List<MessageData> _messages = new List<MessageData>();

		float cellHeight = 1;

        public void Init()
        {
            _tableView.CellPrefab = _msgCellPrefab;
            _tableView.Init(this, this);
            _tableView.GetComponent<TableViewScroll>().enabled = false;

            _tableView.onSelectCell.AddListener (OnScrollTable);
            _tableView.onSelectCell.AddListener (OnChooseCurrentCell);

            _tableView.ScrollToCell (_tableView.NumberOfRows () - 1);

            LoadTablePosition(_tableView);
            _answerController.Init(this);
        }

        private void OnDestroy()
        {
            _tableView.onSelectCell.RemoveAllListeners();
        }

        #region ITableViewDataSource

        public int NumberOfRowsInTableView(TableView tableView)
        {
            return _messages.Count;
        }

        public float SizeForRowInTableView(TableView tableView, int row)
        {
			cellHeight = (_tableView.GetComponent<RectTransform>().rect.size.y) / 4.5f;
			return cellHeight;
        }

        public TableViewCell CellForRowInTableView(TableView tableView, int row)
        {
            TableViewCell cell = tableView.ReusableCellForRow(row);
			cell.GetComponent<RectTransform>().sizeDelta = new Vector2(cell.GetComponent<RectTransform>().sizeDelta.x, cellHeight);
			cell.GetComponent<LayoutElement> ().minHeight = cellHeight;
			cell.GetComponent<LayoutElement> ().preferredHeight = cellHeight;
            MessageCell msgCell = cell.GetComponent<MessageCell>();
            //            displayItem.BaseSize = Constants.SEAL_SIZE;
            //            displayItem.IconPanelActive = false;
            //            ItemInfo itemInfo = _sealsItems[row].ItemInfo;
            //            displayItem.SetItemInfo(itemInfo);
            //              displayItem->setActive(true);
            msgCell.UpdateMsg(_messages[row]);

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
            //            _tableView.ScrollToCell (row, 0.5f);
            //          Log.Write("TableViewDidSelectCellForRow : " + row);
        }

        #endregion

        private void LoadTablePosition(TableView table)
        {
            if (table == null)
                return;

    //        string tableName = GetCurrentTableName(table);
    //        if (tableName != "") {
    //            int index = GameUtils.SlidersIndex.GetSliderIndex(tableName);
    //            if (index >= 0 && index < table.NumberOfRows()) {
    //                table.ScrollToCell(index);
    //            }
    //        }
        }

        private void OnScrollTable(object data)
        {
    //        if (_currentTable == null) {
    //            return;
    //        }
    //
    //        SaveSliderPosition();
    //
    //        if (_currentTable == _perksTable) {
    //            SliderItemsTouchEnable();
    //        }
        }

        private void OnChooseCurrentCell(object data)
        {
    //        if (_currentTable == null || (_currentTable != _tricksTable && _currentTable != _achievementsTable && _currentTable != _sealsTable)) {
    //            return;
    //        }
    //
    //        if (_currentTable != _sealsTable) {
    //            ProfileCell cell = (ProfileCell)_currentTable.SelectedCell;
    //
    //            if (cell != null) {
    //                SubItem subItem = cell.GetFirstIcon ();
    //                subItem.Choose ();
    //            }
    //        } else {
    //            ShopTableViewCell item = (ShopTableViewCell)_currentTable.SelectedCell;
    //            if (item != null) {
    //                ItemInfo itemInfo = item.ItemInfo;
    //                _rightPanel.SetItemInfo(itemInfo);
    //            }
    //        }
        }

        public void SetCurrentContact(int contactId)
        {
            _messages.Clear();
            List<Question> sendedQuestions = AppController.Instance.MessagesController.GetMessagesByContact(contactId);

            for(int i = 0; i < sendedQuestions.Count; i++) 
            {
                Question sendedQuestion = sendedQuestions[i];
                
                MessageData newContactMsg = new MessageData(false, sendedQuestion.text);
                _messages.Add(newContactMsg);

                if (sendedQuestion.GetAnswer() != null)
                {
					bool playerFail = sendedQuestion.AnswerStr != sendedQuestion.GetAnswer ().text;
					MessageData newMyMsg = new MessageData(true, sendedQuestion.AnswerStr, playerFail);
                    _messages.Add(newMyMsg);
                }
            }


            _tableView.ReloadData();

            if (sendedQuestions.Count == 0)
            {
                _tableView.ScrollToCell (_messages.Count - 2);
                _answerController.gameObject.SetActive(false);
                return;
            }

            if (sendedQuestions[sendedQuestions.Count - 1].GetAnswer() != null)
            {
                _tableView.ScrollToCell (_messages.Count - 2);
                _answerController.gameObject.SetActive(false);
                return;
            }

            _tableView.ScrollToCell (_messages.Count - 1);
            _answerController.gameObject.SetActive(true);
            _answerController.SetAnswers(sendedQuestions[sendedQuestions.Count - 1].AllAnswers, sendedQuestions.Count);
        }

        public void ReloadCurrentDialog()
        {
            int currentContactID = AppController.Instance.GetCurrentContactID();
            SetCurrentContact(currentContactID);
        }
    }

	public class MessageData
	{
		public bool IsMine;
		public string Text;
		public bool PlayerFail;

		public MessageData(bool isMine, string text, bool playerFail = false)
		{
			IsMine = isMine;
			Text = text;
			PlayerFail = playerFail;
		}
	}
}