﻿using System;
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
    public static class DialoguesManager
    {
        private static List<Statement> statements; //lista wypowiedzi
        private static List<Statement> lerningStatements; //lista wypowiedzi w trakcie uczenia sie
        private static List<Statement> openingStatements; //lista wypowiedzi przy otwieraniu dzwi od lab
        private static List<Statement> laserStatements;//gdy podejdziemy do lasera 
        private static List<Statement> onKeystatements; //lista wypowiedzi uruchamiana przyciskiem
        private static List<Statement> hackingStatements;
        public static List<int> statementTime;
        public static List<int> lerningTime;
        public static List<int> openingTime;
        public static List<int> laserTime;
        public static List<int> onKeyTime;
        public static List<int> hackingTime;

        private static XMLDialogues xmlDialogues;
        private static int acctualStatementNumber; //ktora jest teraz wypowiedz (z listy wypowiedzi)
        private static int maxLine; //ostatnia linia dialogu
        private static int acctualLineOfStatementCounter; //miejsce w liscie Linii dialogu
        public static int acctualLineOfStatementBefore; //poprzednia linijka
        private static bool isNextStatements; //czy mamy jeszcze jakies dialogi?
        private static bool isCorrectRoom; //czy to odpowiedni pokoj
        private static bool isStart; //czy to poczatek programu, pozniej pomaga w okresleniu czy opuscilismy pokoj przed koncem dialogu
        private static int actuallLineOfLerningStatement; //która linia dialogowa z nauki
        private static int actualLineOnPress; //która linia dialogowa z tych na przycisk
        private static int actualLineLerning; //ktora linia dialogowa z tych w trakcie nauki
        private static int actualLineOpening; //ktora linia dialogowa z tych w trakcie otwierania drzwi do lab
        private static int actualLineHacking;
        private static int actualLineLaser;
        private static int LerningLinesCount; //dlugosc (ilosc linii) wypowiedzi o uczeniu sie
        private static int OpeningLinesCount; //dlugosc (ilosc linii) wypowiedzi o otwieraniu drzwi
        private static int LaserLinesCount;
        private static int HackingLinesCount;
        private static int OnPressLinesCount; //dlugosc (ilosc linii) wypowiedzi na przycisk
        private static bool isPressed;
        private static bool isLerning;
        private static bool isOpening;
        private static bool isLaser;
        private static bool isHacking;

        public static bool IsHacking
        {
            get { return DialoguesManager.isHacking; }
            set { DialoguesManager.isHacking = value; }
        }
        public static List<Statement> HackingStatements
        {
            get { return DialoguesManager.hackingStatements; }
            set { DialoguesManager.hackingStatements = value; }
        }
        public static int ActualLineHacking
        {
            get { return DialoguesManager.actualLineHacking; }
            set { DialoguesManager.actualLineHacking = value; }
        }
        public static bool IsLaser
        {
            get { return DialoguesManager.isLaser; }
            set { DialoguesManager.isLaser = value; }
        }
        public static List<Statement> LaserStatements
        {
            get { return DialoguesManager.laserStatements; }
            set { DialoguesManager.laserStatements = value; }
        }
        public static int ActualLineOpening
        {
            get { return DialoguesManager.actualLineOpening; }
            set { DialoguesManager.actualLineOpening = value; }
        }
        public static bool IsOpening
        {
            get { return DialoguesManager.isOpening; }
            set { DialoguesManager.isOpening = value; }
        }
        public static int ActualLineLaser
        {
            get { return DialoguesManager.actualLineLaser; }
            set { DialoguesManager.actualLineLaser = value; }
        }
        public static int ActualLineLerning
        {
            get { return DialoguesManager.actualLineLerning; }
            set { DialoguesManager.actualLineLerning = value; }
        }
        public static List<Statement> OpeningStatements
        {
            get { return DialoguesManager.openingStatements; }
            set { DialoguesManager.openingStatements = value; }
        }
        public static bool IsLerning
        {
            get { return DialoguesManager.isLerning; }
            set { DialoguesManager.isLerning = value; }
        }

        public static int ActualLineOnPress
        {
            get { return DialoguesManager.actualLineOnPress; }
            set { DialoguesManager.actualLineOnPress = value; }
        }
        public static bool IsPressed
        {
            get { return isPressed; }
            set { isPressed = value; }
        }
        public static bool IsCorrectRoom
        {
            get { return isCorrectRoom; }
        }
        public static int AcctualLineOfStatementCounter
        {
            get { return acctualLineOfStatementCounter; }
        }
        private static TimeSpan timeToNextLine;
        private static TimeSpan timeToEnd;
        private static TimeSpan timeToNextLerning;
        private static TimeSpan timeToNextOpening;
        private static TimeSpan timeToNextLaser;
        private static TimeSpan timeToNextHacking;
        public static int AcctualStatementNumber
        {
            get { return acctualStatementNumber; }
        }
        internal static List<Statement> Statements
        {
            get { return statements; }
            set { statements = value; }
        }
        public static List<Statement> LerningStatements
        {
            get { return DialoguesManager.lerningStatements; }
            set { DialoguesManager.lerningStatements = value; }
        }
        public static List<Statement> OnKeystatements
        {
            get { return DialoguesManager.onKeystatements; }
            set { DialoguesManager.onKeystatements = value; }
        }


        public static void InitializeDialoguesManager()
        {
            xmlDialogues = new XMLDialogues();
            statements = new List<Statement>();
            onKeystatements = new List<Statement>();
            lerningStatements = new List<Statement>();
            openingStatements = new List<Statement>();
            laserStatements = new List<Statement>();
            hackingStatements = new List<Statement>();
            statementTime = new List<int>();
            lerningTime = new List<int>();
            openingTime = new List<int>();
            hackingTime = new List<int>();
            laserTime = new List<int>();
            onKeyTime = new List<int>();

            xmlDialogues = DeserializeFromXML();
            foreach (Statement statement in xmlDialogues.statements)
            {
                if (statement.placeToShow != "OnKey" && statement.placeToShow != "PaloLerning"
                    && statement.placeToShow != "ClosedDoor" && statement.placeToShow != "Laser"
                    && statement.placeToShow != "Hacking")
                {
                    statements.Add(statement);
                }
                else if (statement.placeToShow == "OnKey")
                {
                    OnKeystatements.Add(statement);
                }
                else if (statement.placeToShow == "PaloLerning")
                {
                    lerningStatements.Add(statement);
                }
                else if (statement.placeToShow == "ClosedDoor")
                {
                    openingStatements.Add(statement);
                }
                else if (statement.placeToShow == "Laser")
                {
                    laserStatements.Add(statement);
                }
                else if (statement.placeToShow == "Hacking")
                {
                    hackingStatements.Add(statement);
                }
            }
            OnPressLinesCount = OnKeystatements[0].dialogLines.Line.Count;
            LerningLinesCount = lerningStatements[0].dialogLines.Line.Count;
            OpeningLinesCount = openingStatements[0].dialogLines.Line.Count;
            LaserLinesCount = laserStatements[0].dialogLines.Line.Count;
            HackingLinesCount = hackingStatements[0].dialogLines.Line.Count;
            timeToNextLine = TimeSpan.Zero;
            timeToEnd = TimeSpan.Zero;
            timeToNextLerning = TimeSpan.Zero;
            timeToNextOpening = TimeSpan.Zero;
            timeToNextLaser = TimeSpan.Zero;
            timeToNextHacking = TimeSpan.Zero;
            acctualLineOfStatementCounter = 0;
            acctualLineOfStatementBefore = -1;
            actuallLineOfLerningStatement = 0;
            acctualStatementNumber = 0;
            actualLineLerning = 0;
            actualLineOpening = 0;
            actualLineLaser = 0;
            isNextStatements = true;
            isStart = true;
            isPressed = false;
            isLerning = false;
            isOpening = false;
            isLaser = false;
            isHacking = false;
        }
        private static XMLDialogues DeserializeFromXML()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(XMLDialogues));
            TextReader textReader = new StreamReader(@"../../../../ElonsRiotContent/XML/dialogi.xml");
            XMLDialogues tmpGO;
            tmpGO = (XMLDialogues)deserializer.Deserialize(textReader);
            textReader.Close();
            return tmpGO;
        }
        //sprawdza w jakim pokoju jest Elon i wyswietla dialog jesli taki jest dostępny dla tego pokoju
        public static void checkStatements()
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
        public static void mixDailogues()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;
            Random random = new Random(secondsSinceEpoch);
            ///  List<Statement> tmp = lerningStatements;

            for (int i = 0; i < lerningStatements[0].dialogLines.Line.Count; i++)
            {
                int index1 = random.Next(0, lerningStatements[0].dialogLines.Line.Count - 1);
                int index2 = random.Next(0, lerningStatements[0].dialogLines.Line.Count - 1);
                string tmpSta = lerningStatements[0].dialogLines.Line[index1];
                lerningStatements[0].dialogLines.Line[index1] = lerningStatements[0].dialogLines.Line[index2];
                lerningStatements[0].dialogLines.Line[index2] = tmpSta;
            }
            //  lerningStatements = tmp;
        }
        //oblicza czy mozna juz zmienic na kolejna linie dialogu
        public static void withLine(GameTime gameTime)
        {
            if (isNextStatements && isCorrectRoom)
            {
                timeToNextLine += gameTime.ElapsedGameTime;
                if (timeToNextLine > TimeSpan.FromSeconds(statements[acctualStatementNumber].dialogTimes.Times[acctualLineOfStatementCounter]))  //co 5 sekund zmienia sie tekst
                {
                    if (acctualLineOfStatementCounter < maxLine - 1)
                    {
                        acctualLineOfStatementBefore = acctualLineOfStatementCounter;
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
                            Console.WriteLine(acctualStatementNumber);
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
            /*    if (isCorrectRoom == false && isStart == false) 
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
                }*/

            if (isPressed == true)
            {
                timeToEnd += gameTime.ElapsedGameTime;
                if (timeToEnd > TimeSpan.FromSeconds(onKeystatements[0].dialogTimes.Times[actualLineOnPress]))  //co 5 sekund zmienia sie tekst
                {
                    if (actualLineOnPress < OnPressLinesCount - 1)
                    {
                        actualLineOnPress++;
                        timeToEnd = TimeSpan.Zero;
                        isPressed = false;
                    }
                }
            }
            if (isLerning == true)
            {
                timeToNextLerning += gameTime.ElapsedGameTime;
                if (timeToNextLerning > TimeSpan.FromSeconds(lerningStatements[0].dialogTimes.Times[actualLineLerning]))  //co 5 sekund zmienia sie tekst
                {
                    if (actualLineLerning < LerningLinesCount - 1)
                    {
                        actualLineLerning++;
                        timeToNextLerning = TimeSpan.Zero;
                    }
                    else
                    {
                        actualLineLerning = 0;
                        timeToNextLerning = TimeSpan.Zero;
                        isLerning = false;
                    }
                }
            }
            if (isOpening == true)
            {
                timeToNextOpening += gameTime.ElapsedGameTime;
                if (timeToNextOpening > TimeSpan.FromSeconds(openingStatements[0].dialogTimes.Times[actualLineOpening]))
                {
                    if (actualLineOpening < OpeningLinesCount - 1)
                    {
                        actualLineOpening++;
                        timeToNextOpening = TimeSpan.Zero;
                    }
                    else
                    {
                        actualLineOpening = 0;
                        timeToNextOpening = TimeSpan.Zero;
                        isOpening = false;
                    }
                }
            }
            if (isLaser == true && laserStatements.Count != 0)
            {
                timeToNextLaser += gameTime.ElapsedGameTime;
                if (timeToNextLaser > TimeSpan.FromSeconds(laserStatements[0].dialogTimes.Times[actualLineLaser]))
                {
                    if (actualLineLaser < LaserLinesCount - 1)
                    {
                        actualLineLaser++;
                        timeToNextLaser = TimeSpan.Zero;
                    }
                    else
                    {
                        actualLineLaser = 0;
                        timeToNextLaser = TimeSpan.Zero;
                        isLaser = false;
                    }
                }
            }
            if (isHacking == true)
            {
                timeToNextHacking += gameTime.ElapsedGameTime;
                if (timeToNextHacking > TimeSpan.FromSeconds(hackingStatements[0].dialogTimes.Times[actualLineHacking]))
                {
                    if (actualLineHacking < HackingLinesCount - 1)
                    {
                        actualLineHacking++;
                        timeToNextHacking = TimeSpan.Zero;
                    }
                    else
                    {
                        actualLineHacking = 0;
                        timeToNextHacking = TimeSpan.Zero;
                        isHacking = false;
                    }
                }
            }



        }

    }
}
