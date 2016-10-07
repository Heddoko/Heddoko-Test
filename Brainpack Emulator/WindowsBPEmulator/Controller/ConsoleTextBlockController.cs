// /**
// * @file ConsoleTextBlockController.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System.Windows;
using System.Windows.Controls;

namespace WindowsBPEmulator.Controller
{
    public class ConsoleTextBlockController
    {
        TextBlock TextBlock;
        public int Count = 1;

        public ConsoleTextBlockController(TextBlock vBlock)
        {
            TextBlock = vBlock;
        }
        public void AddMsg(string vMsg)
        {
            TextBlock.Text += Count++ + " " + vMsg + "\r\n";
        } 

        public void Clear(object vSender, RoutedEventArgs vE)
        {
            TextBlock.Text = string.Empty;
            Count = 1;
        }
    }
}