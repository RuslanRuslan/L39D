﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Brolton.Utils;

namespace LudumDare39
{
    public enum AnswerLength
    {
        Short,
        Long
    }

    public enum AnswerColor
    {
        None,
        Red,
        Yellow,
        Green
    }
    
    public class Answer
    {
        public AnswerColor color;      // Color="Red" 
        public AnswerLength length;    // Length="Short" 
        public int points;             // Points="10"
        public string text;            // Text="Pizza?0_o" 
        public string NextQuestionId;     // NextId="2a"
        public Question ParentQuestion;

        public void Parse(XmlNode node)
        {
            string colorStr = node.Attributes["Color"].AsString("");
            switch (colorStr)
            {
                case "Red":
                    color = AnswerColor.Red;
                    break;
                case "Yellow":
                    color = AnswerColor.Yellow;
                    break;
                case "Green":
                    color = AnswerColor.Green;
                    break;
                default:
                    color = AnswerColor.None;
                    break;
            }

//            length = node.Attributes["Length"].AsString("");
            points = node.Attributes["Points"].AsInt(0);
            text = node.Attributes["Text"].AsString("");
            NextQuestionId = node.Attributes["NextId"].AsString("");
        }
    }

    public class Question
    {
        public string Id;                                  // Id="1"
        public string text;                             // Text="Luv, pizza's ready^^" 
        public List<Answer> AllAnswers = new List<Answer>();
        int _answerId = -1;
        public string AnswerStr = "";

        public void Parse(XmlNode node)
        {
            Id = node.Attributes["Id"].AsString("");
            text = node.Attributes["Text"].AsString("");
            foreach (XmlNode answerNode in node.ChildNodes)
            {
                Answer newAnswer = new Answer();
                newAnswer.Parse(answerNode);
                newAnswer.ParentQuestion = this;
                AllAnswers.Add(newAnswer);
            }
        }

        public Answer GetAnswer()
        {
            if (_answerId < 0)
            {
                return null;
            }
            return AllAnswers[_answerId];

        }

        public void SetAnswerId(int answerId)
        {
            _answerId = answerId;
        }

        public void SetAnswerText(string answerStr)
        {
            AnswerStr = answerStr;
        }
    }
    
    public class MessageController
    {
        public List<ContactData> AllContacts = new List<ContactData>();

        public void Init()
        {
//            XmlDocument doc= new XmlDocument();
//            doc.Load( Application.dataPath + "/dialogs_with_ids.xml" );
//            //            XmlDocument _docQuests = XmlUtils.OpenXMLDocument(SF2Paths.GameData, "dialogs_with_ids.xml");
//            ParseXml(doc);

            XmlDocument doc= new XmlDocument();
            doc.Load( Application.dataPath + "/gamedata/contacts.xml" );
            ParseContacts(doc);
        }

        void ParseContacts(XmlDocument doc)
        {
            var nodeRoot = doc["Contacts"];
            foreach (XmlNode contactNode in nodeRoot.ChildNodes)
            {
                ContactData newContact = new ContactData();
                newContact.Parse(contactNode);
                AllContacts.Add(newContact);
            }
        }

        public List<Question> GetMessagesByContact(int contactId)
        {
            return AllContacts[contactId].SendedQuestions;
        }
    }
}

