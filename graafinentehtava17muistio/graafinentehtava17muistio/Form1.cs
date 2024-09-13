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
using System.Drawing.Printing; // Käytämme tätä kirjastoa

namespace graafinentehtava17muistio
{
    public partial class NotepadFR : Form
    {
        private OpenFileDialog OpenFileDialog; //Avaaminen
        private SaveFileDialog SaveFileDialog; // Tallentaminen
        private FontDialog FontDialog; //Fontti eli kirjasin
        public NotepadFR()
        {
            InitializeComponent();
        }

        private void UusiTiedosto() // Oma funktio, joka on yksityinen (private), eikä palauta mitään (void)
        {
            try
            {
                if (!string.IsNullOrEmpty(TekstiTB.Text))// Testataan onko tekstiä; eli jos EI ole tyhjä tai nolla, niin sitten tapahtuu seuraavaa:
                {
                    MessageBox.Show("Sinun tulee tallentaa ensin!");
                }
                else // Mitä tapahtuu jos on tyhjä
                {
                    TekstiTB.Text = string.Empty;
                    Text = "Nimetön"; // Oletuksena nimetön
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Virhe " + ex); // tulee tieto virheesta ja mikä virhe (ex) tulee
            }
        }

        private void TallennaTiedosto()
        {
            try
            {
                if (!string.IsNullOrEmpty(TekstiTB.Text))
                {
                    SaveFileDialog SaveFileDialog = new SaveFileDialog(); // Uusi olio
                    SaveFileDialog.Filter = "Tekstitiedosto (*.txt) | *.txt | Rikas testiformaatti (*.rtf) | *.rtf"; //Tallennusmuoto
                    if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(SaveFileDialog.FileName, TekstiTB.Text);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Virhe " + ex);
            }
        }

        private void AvaaTiedosto()
        {
            try
            {
                OpenFileDialog = new OpenFileDialog(); // Uusi olio
                if (OpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    TekstiTB.Text = File.ReadAllText(OpenFileDialog.FileName);
                    Text = OpenFileDialog.FileName;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Virhe avattaessa tiedostoa " + ex);
            }
        }




        private void NotepadFR_Load(object sender, EventArgs e)
        {
            FontDialog = new FontDialog(); // Uusi olio
        }

        private void uusiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UusiTiedosto(); // Kutsutaan UusiTiedosto() -funktiota
        }

        private void avaaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AvaaTiedosto();
        }

        private void tallennaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TallennaTiedosto(); // Kutsutaan TallennaTiedosto() 
        }

        private void lopetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try // Voit tehdä koodin suoraan tänne, eli molemmat tavat oikein; tämä tapa tai funktiolla, jota kutsutaan, kuten ylläolevissa on tehty
            {
                if (!string.IsNullOrEmpty(TekstiTB.Text))
                {
                    TallennaTiedosto(); // Eli jos ei ole tyhjä, kutsutaan TallennaTiedosto -funktiota
                }
                else
                {
                    this.Close(); // Jos on tyhjä, suljetaan
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Virhe " + ex);
            }
        }

        private void kirjasinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (FontDialog.ShowDialog() == DialogResult.OK)
                {
                    TekstiTB.Font = FontDialog.Font; // Fontti muutetaan tällä komennolla (huom! kaikki fontti)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Virhe " + ex);
            }
        }

        private void tallennaNimelläToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog ttk = new SaveFileDialog())
            {
                if (ttk.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter jonokirjoittaja = new StreamWriter(ttk.FileName))
                    {
                        jonokirjoittaja.WriteAsync(TekstiTB.Text);
                    }
                }
            }
        }


        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(TekstiTB.Text, TekstiTB.Font, Brushes.Black, 12, 10); // Tässä määritellään tulostuksen asetuksia, kuten fontti, koko ja mistä mihin tulostetaan
        }

        private void tulostuksenEsikatseluToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void tulostaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TekstiTB.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TekstiTB.Redo();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TekstiTB.Copy();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TekstiTB.Cut();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TekstiTB.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TekstiTB.SelectAll();
        }

        private void selectedTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TekstiTB.SelectedText = ""; // Tämä tarkoittaa, että poistetaan valittu teksti
        }

        private void TekstiTB_TextChanged(object sender, EventArgs e)
        {
            if (TekstiTB.Text.Length > 0) // Tässä määritelemme kopioinnin ja leikkaamisen mahdollisiksi vain kun tekstiä on
            {
                copyToolStripMenuItem.Enabled = true;
                cutToolStripMenuItem.Enabled = true;
            }
            else
            {
                copyToolStripMenuItem.Enabled = false;
                cutToolStripMenuItem.Enabled = false;
            }
        }

        private void tekstinRivitysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(tekstinRivitysToolStripMenuItem.Checked == true) //Jos rivitä teksti -painike on käytössä, rivitetään teksti ikkunan mukaisesti
            {
                tekstinRivitysToolStripMenuItem.Checked = false;
                TekstiTB.WordWrap = false;
            }
            else 
            {
                tekstinRivitysToolStripMenuItem.Checked = true;
                TekstiTB.WordWrap = true;
            }
        }

        private void tekstinKorostusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TekstiTB.SelectionBackColor = Color.Yellow;
        }

        private void tietoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tietoa tietoa = new Tietoa();
            tietoa.ShowDialog();
        }
    }

}
