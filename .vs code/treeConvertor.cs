using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace treeConvertor
{
    public partial class treeConvertor : Form
    {
        Dictionary<string,string> text2tree = new Dictionary<string, string>();
        Dictionary<string,string> tree2text = new Dictionary<string, string>();
        bool textToTree = true;
        string betweenLetters = ":tanabata_tree:";

        public treeConvertor()
        {
            InitializeComponent();
            string[] characters = System.IO.File.ReadAllLines(@"config.ini");
            foreach(string x in characters) {
                text2tree.Add(x.Split('>')[0],toUnicode(x.Split('>')[1]));
                tree2text.Add(x.Split('>')[1], x.Split('>')[0]);
            }
        }

        string toUnicode(string txt)
        {
            return txt.Replace(":palm_tree:", "🌴").Replace(":evergreen_tree:", "🌲").Replace(":christmas_tree:", "🎄").Replace(":deciduous_tree:", "🌳").Replace(":tanabata_tree:", "🎋");
        }

        double getDensity(string txt)
        {
            double palmD = txt.Length - txt.Replace(":palm_tree:", "").Length;
            double everD = txt.Length - txt.Replace(":evergreen_tree:", "").Length;
            double decideousD = txt.Length - txt.Replace(":deciduous_tree:", "").Length;
            double tanabataD = txt.Length - txt.Replace(":tanabata_tree:", "").Length;
            return (palmD + everD + decideousD + tanabataD) / txt.Length;
        }

        void changeTranslation(bool state)
        {
            textToTree = state;
            if (textToTree) {
                label1.Text = "Text";
                label2.Text = "Tree";
            }
            else {
                label1.Text = "Tree";
                label2.Text = "Text";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string text = richTextBox1.Text.Replace("\n", " ").ToLower();
            double a = getDensity(text);
            if (checkBox1.Checked) {
                if (getDensity(text) > 0.50 && textToTree) {
                    changeTranslation(false);
                } else if (getDensity(text) <= 0.50 && !textToTree) {
                    changeTranslation(true);
                }
            }
            string result = "";
            if (textToTree) {
                foreach (char x in text) {
                    result += (text2tree.ContainsKey(x.ToString()) ? text2tree[x.ToString()] : x.ToString()) + toUnicode(betweenLetters);
                }
                richTextBox2.Text = result;
            } else {
                string emoji = "";
                foreach (char x in text) {
                    emoji += x;
                    if (x == ':' && emoji.Contains(betweenLetters)) {
                        emoji = emoji.Replace(betweenLetters,"");
                        result += (tree2text.ContainsKey(emoji.ToString()) ? tree2text[emoji.ToString()] : emoji.ToString());
                        emoji = "";
                    }
                }
                richTextBox2.Text = result;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            changeTranslation(!textToTree);
        }
    }
}
