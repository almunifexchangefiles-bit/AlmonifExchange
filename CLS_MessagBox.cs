using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlmonifExchange
{
    public static class CLS_MessagBox
    {
        public static void Messag_Data(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool Messag_Data_return_Check(string title, string type, string message)
        {
            DialogResult result = MessageBox.Show(
                message,
                title,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }
    }
}