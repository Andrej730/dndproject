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
using System.Xml.Serialization;

namespace dndproject
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeApp();
        }

        private void InitializeApp()
        {
            string profiles_path = @"profiles.txt";

            if (System.IO.File.Exists(profiles_path) == false)
                System.IO.File.Create(profiles_path);

            //StreamWriter write_text;
            //write_text = System.IO.File.AppendText(profiles_path);
            //write_text.WriteLine("Николай");
            //write_text.WriteLine("Алексей");
            //write_text.Close();

            characters = System.IO.File.ReadAllLines(@"profiles.txt");
            characters_comboBox.Items.AddRange(characters);

        }

        private void applyCharacter(Character ch)
        {
            name_textBox.Text = ch.name;
            race_textBox.Text = ch.race;
            name_textBox.Text = ch.name;
            race_textBox.Text = ch.race;
            class_textBox.Text = ch.character_class;
            ideology_textBox.Text = ch.ideology;
            religion_textBox.Text = ch.religion;
            armour_type_comboBox.Text = ch.armour_type;
            shield_type_comboBox.Text = ch.shield_type;
            strength_textBox.Text = ch.strength;
            agility_textBox.Text = ch.agility;
            intellect_textBox.Text = ch.intellect;
            will_textBox.Text = ch.will;
            charisma_textBox.Text = ch.charisma;
            weapon_richTextBox.Text = ch.weapon;
            special_talent_richTextBox.Text = ch.special_talent;
            inventory_richTextBox.Text = ch.inventory;
            skills_richTextBox.Text = ch.skills;
            description_richTextBox.Text = ch.description;
        }

        private void saveLastCharacter()
        {
            Character ch = new Character();

            ch.name = name_textBox.Text;
            ch.race = race_textBox.Text;
            ch.name = name_textBox.Text;
            ch.race = race_textBox.Text;
            ch.character_class = class_textBox.Text;
            ch.ideology = ideology_textBox.Text;
            ch.religion = religion_textBox.Text;
            ch.armour_type = armour_type_comboBox.Text;
            ch.shield_type = shield_type_comboBox.Text;
            ch.strength = strength_textBox.Text;
            ch.agility = agility_textBox.Text;
            ch.intellect = intellect_textBox.Text;
            ch.will = will_textBox.Text;
            ch.charisma = charisma_textBox.Text;
            ch.weapon = weapon_richTextBox.Text;
            ch.special_talent = special_talent_richTextBox.Text;
            ch.inventory = inventory_richTextBox.Text;
            ch.skills = skills_richTextBox.Text;
            ch.description = description_richTextBox.Text;

            string profile_path = String.Format(@"profiles/{0}.txt", previous_character);
            XmlSerializer ser = new XmlSerializer(typeof(Character));
            TextWriter writer = new StreamWriter(profile_path);

            ser.Serialize(writer, ch);
            writer.Close();
        }


        private void clearAllFields()
        {
            name_textBox.Text = "";
            race_textBox.Text = "";
            name_textBox.Text = "";
            race_textBox.Text = "";
            class_textBox.Text = "";
            ideology_textBox.Text = "";
            religion_textBox.Text = "";
            armour_type_comboBox.Text = "";
            shield_type_comboBox.Text = "";
            strength_textBox.Text = "";
            agility_textBox.Text = "";
            intellect_textBox.Text = "";
            will_textBox.Text = "";
            charisma_textBox.Text = "";
            weapon_richTextBox.Text = "";
            special_talent_richTextBox.Text = "";
            inventory_richTextBox.Text = "";
            skills_richTextBox.Text = "";
            description_richTextBox.Text = "";
        }

        private void drawRollModifier()
        {
            if (roll_modifier != 0) {
                if (roll_modifier > 0) {
                    label15.Text = this.roll_modifier.ToString();
                }
                else
                {
                    label16.Text = this.roll_modifier.ToString();
                }
            }
            else
            {
                label15.Text = " ";
                label16.Text = " ";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox6.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.roll_modifier + 1 < this.max_roll)
                this.roll_modifier += 1;
            drawRollModifier();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.roll_modifier - 1 > -this.max_roll)
                this.roll_modifier -= 1;
            drawRollModifier();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var next_index = Array.IndexOf(dices, max_roll) + 1;
            max_roll = dices[next_index == dices.Length ? 0 : next_index];

            if (roll_modifier + 1 >= max_roll) {
                roll_modifier = roll_modifier > 0 ? max_roll - 1 : -max_roll + 1;
                drawRollModifier();
            }

            button6.Text = max_roll.ToString();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            var roll = rnd.Next(1, max_roll) + roll_modifier;
            if (roll > max_roll)
            {
                roll = max_roll;
            }
            else if (roll < 1)
            {
                roll = 1;
            }
            label14.Text = roll.ToString();
            richTextBox6.AppendText(roll.ToString()+"\n");
        }

        private void richTextBox6_TextChanged(object sender, EventArgs e)
        {
            richTextBox6.SelectionStart = richTextBox6.Text.Length;
            richTextBox6.ScrollToCaret();
        }

        private void characters_comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (previous_character != "") {
                saveLastCharacter();
            }
            Character ch;
            string character_name = (string) characters_comboBox.SelectedItem;
            string profile_path = String.Format("profiles/{0}.txt", character_name);
            if (System.IO.File.Exists(profile_path))
            {
                XmlSerializer ser = new XmlSerializer(typeof(Character));
                TextReader reader = new StreamReader(profile_path);
                ch = (Character) ser.Deserialize(reader);
                reader.Close();
                applyCharacter(ch);
            }
            else
            {
                MessageBox.Show("Cannot find file: " + profile_path);
                clearAllFields();
            }
            previous_character = character_name;
        }


        // App variables
        //Characters
        private string[] characters;
        private string previous_character = "";
        //Roll
        private int roll_modifier = 0;
        private int max_roll = 10;
        private int[] dices = new int[5] { 6, 10, 12, 20, 100 };

        private void button7_Click(object sender, EventArgs e)
        {
            int damage = Int32.Parse(damage_textBox.Text);
            double damageModifier = Double.Parse(damageModifier_textBox.Text);
            double damageResult = damage * damageModifier;
            damageModified_textBox.Text = damageResult.ToString();
        }

        private void StrengthItemMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Бонус силы\n10 - -1 к кубу урона по тебе\n15 - -1 ход отката на определенные физические атаки (боевая выносливость)\n20 - -2 к кубу урона по тебе\n25 - -2 отката на определенные физические атаки (боевая выносливость)");        
        }

        private void AgilityItemMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Бонус ловкости\n10 - +1 к кубам на шанс физ статус эффекта от ловкости\n15 - +1 к увороту (-1 к кубам на попадания врагов)\n20 - +2 к кубам на шанс физ статус эффекта от ловкости\n25 - +2 к увороту (-2 к кубам на попадания врагов)");
        }

        private void IntellectItemMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Интеллект\n10 - -1 к продолжительности магического статус эффекта\n15 - +1 к шансу магического статус эффекта\n20 - -2 к продолжительности магического статус эффекта \n25 - +2 к шансу магического статус эффекта");
        }

        private void WillItemMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Сила Воли\n10 - -1 к продолжительности эффектов контроля\n15 - +1 к силе эффектов вспомогательных заклинаний\n20 - -2 к продолжительности эффектов контроля\n25 - +2 к силе эффектов вспомогательных заклинаний");
        }

        private void CharismaItemMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Харизма\n10 - +1 к кубам на разговор\n15 - дополнительные возможности при разговоре\n20 - +2 к кубам на разговор\n25 - больше дополнительных возможностей при разговоре");
        }
    }

    public class Character
    {
        public string name;
        public string race;
        public string character_class;
        public string ideology;
        public string religion;
        public string armour_type;
        public string shield_type;
        public string strength;
        public string agility;
        public string intellect;
        public string will;
        public string charisma;
        public string weapon;
        public string special_talent;
        public string inventory;
        public string skills;
        public string description;

        public Character()
        {


        }
    }
}
