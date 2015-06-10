using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ElonsRiot.BSPTree;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
//komentuje
namespace ElonsRiot.Dialogues
{
    public class DialoguesManager
    {
        private List<Statement> statements; //lista wypowiedzi
        private XMLDialogues xmlDialogues;
        private int acctualStatementNumber; //ktora jest teraz wypowiedz (z listy wypowiedzi)
        private int maxLine; //ostatnia linia dialogu
        private int acctualLineOfStatementCounter; //miejsce w liscie Linii dialogu
        private bool isNextStatements; //czy mamy jeszcze jakies dialogi?
        private bool isCorrectRoom; //czy to odpowiedni pokoj
        private bool isStart; //czy to poczatek programu, pozniej pomaga w okresleniu czy opuscilismy pokoj przed koncem dialogu
        public bool IsCorrectRoom
        {
            get { return isCorrectRoom; }
        }
        public int AcctualLineOfStatementCounter
        {
            get { return acctualLineOfStatementCounter; }
        }
        private TimeSpan timeToNextLine;
        public int AcctualStatementNumber
        {
            get { return acctualStatementNumber; }
        }
        internal  List<Statement> Statements
        {
            get { return statements; }
            set { statements = value; }
        }
       
        public DialoguesManager()
        {
            xmlDialogues = new XMLDialogues();
        }
        public void InitializeDialoguesManager()
        {
            xmlDialogues = DeserializeFromXML();
            statements = xmlDialogues.statements;
            timeToNextLine = TimeSpan.Zero;
            acctualLineOfStatementCounter = 0;
            acctualStatementNumber = 0;
            isNextStatements = true;
            isStart = true;
        }
        private XMLDialogues DeserializeFromXML()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(XMLDialogues));
            TextReader textReader = new StreamReader(@"../../../../ElonsRiotContent/XML/dialogi.xml");
            XMLDialogues tmpGO;
            tmpGO = (XMLDialogues)deserializer.Deserialize(textReader);
            textReader.Close();
            return tmpGO;
        }
        //sprawdza w jakim pokoju jest Elon i wyswietla dialog jesli taki jest dostępny dla tego pokoju
        public void checkStatements()
        {
            if (statements[acctualStatementNumber].placeToShow == CreateBSP.CurrentRoom)
            {
                maxLine = statements[acctualStatementNumber].dialogLines.Line.Count;
                isCorrectRoom = true;
            }
            else
            {
                isCorrectRoom = false;
            }
        }
     
        //oblicza czy mozna juz zmienic na kolejna linie dialogu
        public void withLine(GameTime gameTime)
        {
            if (isNextStatements && isCorrectRoom) {
                    timeToNextLine += gameTime.ElapsedGameTime;
                    if (timeToNextLine > TimeSpan.FromSeconds(15))  //co 15 sekund zmienia sie tekst
                    {
                        if (acctualLineOfStatementCounter < maxLine-1) { 
                             acctualLineOfStatementCounter++;
                             timeToNextLine = TimeSpan.Zero;
                         }
                            //przeszlismy juz przez wszystkie linie pojedynczego statement
                        else
                        {
                            acctualLineOfStatementCounter = 0;
                            if (acctualStatementNumber < statements.Count - 1)
                            {
                                acctualStatementNumber++; //przesowamy sie na kolejny obiekt w liscie statement
                            }
                            else
                            {
                                isNextStatements = false; //skonczyly sie wypowiedzi do wyswietlenia
                            }
                            timeToNextLine = TimeSpan.Zero;
                        }
                    }
                    isStart = false;
                }
            //opuscilismy pokoj zanim skonczyl sie dialog
            if (isCorrectRoom == false && isStart == false) 
            {
                acctualLineOfStatementCounter = 0;
                if (acctualStatementNumber < statements.Count - 1)
                {
                    acctualStatementNumber++; //przesowamy sie na kolejny obiekt w liscie statement
                    isStart = true;
                }
                else
                {
                    isNextStatements = false; //skonczyly sie wypowiedzi do wyswietlenia
                }
                timeToNextLine = TimeSpan.Zero;
            }
        }
    } 
}
