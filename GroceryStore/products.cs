﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroceryStore
{
    public partial class products : Form
    {
        public products()
        {
            InitializeComponent();
        }

       

        private void products_Load(object sender, EventArgs e)
        {
            dateTimePicker_Manufacture.Format = DateTimePickerFormat.Custom;
            dateTimePicker_Manufacture.CustomFormat = "yyyy-MM-dd";
            dateTimePicker_Expiration.Format = DateTimePickerFormat.Custom;
            dateTimePicker_Expiration.CustomFormat = "yyyy-MM-dd";

            ProductsGridViewRefresh();
            dataGridView_Products.ReadOnly = true;
            dataGridView_Products.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void ProductsGridViewRefresh() //Refresh datagridview
        {
            string sql = "select id as 'ID', product_name as 'Product Name', product_weight as 'Product Weight', product_origin as 'Product Origin', manufacture_date as 'Manufacture Date', expiration_date as 'Expiration Date', stock as 'Stock', price as 'Price' from products";
            Global.con.Open();
            Global.da = new SqlDataAdapter(sql, Global.con);
            Global.ds = new DataSet();
            Global.da.Fill(Global.ds);
            dataGridView_Products.DataSource = Global.ds.Tables[0];
            Global.con.Close();
        }

        private void RemoveErrors () //Remove error text from labels 
        {
            label_NameError.Text = "";
            label_WeightError.Text = "";
            label_CountryError.Text = "";
            label_StockError.Text = "";
            label_PriceError.Text = "";
        }

        private void button_Add_Click(object sender, EventArgs e) //Add a product to the database
        {
            int nr = 0;
            
            if (string.IsNullOrEmpty(textBox_ProductName.Text))
            {
                label_NameError.Text = "This field cannot be empty!"; nr++;
            }
            else
                label_NameError.Text = "";


            if (string.IsNullOrEmpty(textBox_Weight.Text))
            {
                label_WeightError.Text = "This field cannot be empty!"; nr++;
            }
            else
            if (double.TryParse(textBox_Weight.Text, out _) == false)
            {
                label_WeightError.Text = "Weight needs to be a number!"; nr++;
            }
            else
                label_WeightError.Text = "";

            if(string.IsNullOrEmpty(textBox_Origin.Text))
            {
                label_CountryError.Text = "This field cannot be empty!"; nr++;
            }
            else 
            label_CountryError.Text = "";

            if (string.IsNullOrEmpty(textBox_stock.Text))
            {
                label_StockError.Text = "This field cannot be empty!"; nr++;
            }
            else
            if (int.TryParse(textBox_stock.Text, out _) == false)
            {
                label_StockError.Text = "Weight needs to be a number!"; nr++;
            }
            else
                label_StockError.Text = "";


            if (string.IsNullOrEmpty(textBox_Price.Text))
            {
                label_PriceError.Text = "This field cannot be empty!"; nr++;
            }
            else
           if (double.TryParse(textBox_Price.Text, out _) == false)
            {
                label_PriceError.Text = "Weight needs to be a number!"; nr++;
            }
            else
                label_PriceError.Text = "";

            if (nr > 0)
                return;

            string sql = "insert into products values ('" + textBox_ProductName.Text + "','" + textBox_Weight.Text + "','" + textBox_Origin.Text + "','" + dateTimePicker_Manufacture.Text + "','" + dateTimePicker_Expiration.Text + "','" + textBox_stock.Text + "','"+double.Parse(textBox_Price.Text)+"') ";
            Global.con.Open();
            SqlCommand cmd = new SqlCommand(sql,Global.con);
            cmd.ExecuteNonQuery();
            Global.con.Close();
            ProductsGridViewRefresh();
        }

        private void dataGridView_Products_CellClick(object sender, DataGridViewCellEventArgs e) //Fill textbox - cell click
        {
            RemoveErrors();
            int selectedrowindex = dataGridView_Products.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView_Products.Rows[selectedrowindex];
            
            textBox_ProductName.Text = Convert.ToString(selectedRow.Cells["Product Name"].Value);
            textBox_Weight.Text = Convert.ToString(selectedRow.Cells["Product Weight"].Value);
            textBox_Origin.Text = Convert.ToString(selectedRow.Cells["Product Origin"].Value);
            textBox_stock.Text = Convert.ToString(selectedRow.Cells["Stock"].Value);
            dateTimePicker_Manufacture.Text = Convert.ToString(selectedRow.Cells["Manufacture Date"].Value);
            dateTimePicker_Expiration.Text = Convert.ToString(selectedRow.Cells["Expiration Date"].Value);
            textBox_Price.Text = Convert.ToString(selectedRow.Cells["Price"].Value);
            Global.nr = 1;
        }

        private void button_Clear_Click(object sender, EventArgs e) //Clear textbox
        {
            textBox_ProductName.Clear();
            textBox_Weight.Clear();
            textBox_Origin.Clear();
            textBox_stock.Clear();
            textBox_Price.Clear();
            Global.nr = 0;
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            int selectedrowindex = dataGridView_Products.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView_Products.Rows[selectedrowindex];
            string id = Convert.ToString(selectedRow.Cells["id"].Value);
           
            string sql = "delete from products where id =  "+ id;
            Global.con.Open();
            SqlCommand cmd = new SqlCommand(sql,Global.con);
            cmd.ExecuteNonQuery();
            Global.con.Close();
            ProductsGridViewRefresh();
            textBox_ProductName.Clear();
            textBox_Weight.Clear();
            textBox_Origin.Clear();
            textBox_stock.Clear();
            textBox_Price.Clear();
        } //Delete product

        private void button_Update_Click(object sender, EventArgs e)
        {
            if(Global.nr == 0)
            { MessageBox.Show("You need to select a product first! "); return; }

            int selectedrowindex = dataGridView_Products.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView_Products.Rows[selectedrowindex];
            string id = Convert.ToString(selectedRow.Cells["id"].Value);

            string sql = "update products set product_name ='"+textBox_ProductName.Text+"', product_weight ='" + textBox_Weight.Text +
                "', product_origin ='"+ textBox_Origin.Text +"' , manufacture_date ='"+ dateTimePicker_Manufacture.Text+ 
                "', expiration_date ='"+ dateTimePicker_Expiration.Text+"', stock = '"+ textBox_stock.Text+"', price = "+double.Parse(textBox_Price.Text) +" where id like  "+id;
            MessageBox.Show(sql);
            Global.con.Open(); 
            SqlCommand cmd = new SqlCommand(sql, Global.con);
            cmd.ExecuteNonQuery();
            Global.con.Close();
            ProductsGridViewRefresh();

        } //Update product details

       

        
    }
}
