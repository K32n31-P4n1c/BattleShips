using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; // debug log in the outputs window

namespace BattleShips
{
    public partial class F_BattleShips : Form
    {

        List<Button> PlayerPosition; // List for all the player position buttons
        List<Button> EnemyPosition; // List for all the enemy postion buttons

        int TotalPlayerShips = 3;
        int TotalEnemyShips = 3;

        int Rounds = 10;

        int PlayerScore = 0; // default Player Score
        int EnemyScore = 0; // default Enemy Score

        Random random = new Random(); // Instance from Random class



        public F_BattleShips()
        {
            InitializeComponent();
            LoadButtons();  // Load the buttons for enemy and player to the system
            B_Attack.Enabled = false;     // Disable the player attack button
            CB_EnemyLocation.Text = null; // Enemy Location drop down box set to null
        }

        private void PlayerPick(object sender, EventArgs e)
        {
            // This function will let the player pick 3 positions on the map in the beginning of the game

            if (TotalPlayerShips >0)
            {
                var button = (Button)sender;    // Check which button was clicked
                button.Enabled = false;
                button.Tag = "PlayerShip";
                button.BackColor = System.Drawing.Color.Blue;
                TotalPlayerShips = TotalPlayerShips - 1;
            }

            if (TotalPlayerShips == 0)
            {
                B_Attack.Enabled = true;
                B_Attack.BackColor = System.Drawing.Color.Red;
                L_HelpText.Text = "Pick attack position from the drop down to attack enemy ships";
            }
        }

        private void EnemyPick(object sender, EventArgs e)
        {
            int index = random.Next(EnemyPosition.Count);

            if (EnemyPosition[index].Enabled == true && EnemyPosition[index].Tag == null)
            {
                EnemyPosition[index].Tag = "EnemyShip";
                TotalEnemyShips = TotalEnemyShips - 1;

                Debug.WriteLine("Eneemy Position " + EnemyPosition[index].Text);
            }

            else
            {
                index = random.Next(EnemyPosition.Count);
            }

            if ( TotalEnemyShips < 1)
            {
                T_EnemyPositionPicker.Stop();
            }
        }



        private void AttackEnemyPosition(object sender, EventArgs e)
        {
            if (CB_EnemyLocation.Text != "")
            {
                var AttackPosition = CB_EnemyLocation.Text; // 
                AttackPosition = AttackPosition.ToLower();

                int index = EnemyPosition.FindIndex(a => a.Name == AttackPosition);

                if (EnemyPosition[index].Enabled && Rounds > 0)
                {
                    Rounds = Rounds - 1;
                    L_Rounds.Text = "Rounds " + Rounds;

                    if (EnemyPosition[index].Tag == "EnemyShip")
                    {
                        EnemyPosition[index].Enabled = false;
                        EnemyPosition[index].BackgroundImage = Properties.Resources.fireIcon;
                        EnemyPosition[index].BackColor = System.Drawing.Color.DarkBlue;
                        PlayerScore = PlayerScore + 1;
                        L_PlayerScore.Text = "" + PlayerScore;
                        T_EnemyPlayTimer.Start();
                    }
                    else
                    {
                        EnemyPosition[index].Enabled = false;
                        EnemyPosition[index].BackgroundImage = Properties.Resources.missIcon;
                        EnemyPosition[index].BackColor = System.Drawing.Color.DarkBlue;
                        T_EnemyPlayTimer.Start();
                    }
                }
            }

            else
            {
                MessageBox.Show("Choose a location from the drop down list");
            }
        }


        private void EnemyAttackPlayer(object sender, EventArgs e)
        {
            if (PlayerPosition.Count > 0 && Rounds > 0)
            {
                Rounds = Rounds - 1;
                L_Rounds.Text = "Rounds " + Rounds;

                int index = random.Next(PlayerPosition.Count);
                
                if ( PlayerPosition[index].Tag == "PlayerShip")
                {
                    PlayerPosition[index].BackgroundImage = Properties.Resources.fireIcon;

                    PlayerPosition[index].Enabled = false;
                    PlayerPosition[index].BackColor = System.Drawing.Color.DarkBlue;
                    PlayerPosition.RemoveAt(index);

                    EnemyScore = EnemyScore + 1;
                    L_EnemyScore.Text = "" + EnemyScore;
                    T_EnemyPlayTimer.Stop();
                }
                else
                {
                    PlayerPosition[index].BackgroundImage = Properties.Resources.missIcon;

                    PlayerPosition[index].Enabled = false;
                    PlayerPosition[index].BackColor = System.Drawing.Color.DarkBlue;
                    PlayerPosition.RemoveAt(index);

                    T_EnemyPlayTimer.Stop();
                }
            }
            
            if (Rounds < 1 || PlayerScore > 2 || EnemyScore > 2)
            {
                if (PlayerScore > EnemyScore)
                {
                    MessageBox.Show("You Win", "Winning");
                }
                if (PlayerScore == EnemyScore)
                {
                    MessageBox.Show("No one winds this", "Drow");
                }
                if (EnemyScore > PlayerScore)
                {
                    MessageBox.Show("You suck at this game", "Lost");
                }
            }
        }

        private void LoadButtons()
        {
            PlayerPosition = new List<Button> { B_pA1, B_pA2, B_pA3, B_pA4, B_pB1, B_pB2, B_pB3, B_pB4, B_pC1, B_pC2, B_pC3, B_pC4, B_pD1, B_pD2, B_pD3, B_pD4 };
            EnemyPosition = new List<Button> { a1, a2, a3, a4, b1, b2, b3, b4, c1, c2, c3, c4, d1, d2, d3, d4 };

            for (int i=0; i < EnemyPosition.Count; i++)
            {
                EnemyPosition[i].Tag = null;
                CB_EnemyLocation.Items.Add(EnemyPosition[i].Text);
            }

        }
    }
}
