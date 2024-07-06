using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgoritmoVernam
{
    public partial class Form1 : Form
    {
        String[] TablaNumeros = new string[10];
        String[] TablaNumerosNumber = new string[10];

        String[] TablaLetras = new string[26];
        String[] TablaLetrasNumber = new string[26];

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //--------------------------------INICIO SEPARAR PALABRA EN LETRAS---------------------------//
            string PALABRA = txtpalabra.Text.ToUpper();
            PALABRA = PALABRA.Replace(" ", "");
            string cadena = "";

            int LONGITUD_P = PALABRA.Length;
            int chunkSize = 1;

            String[] ArrayPalabra = new string[LONGITUD_P];

            for (int a = 0; a < LONGITUD_P; a += chunkSize)
            {
                if (a + chunkSize > LONGITUD_P) chunkSize = LONGITUD_P - a;
                cadena = cadena + (PALABRA.Substring(a, chunkSize)) + " - ";

                ArrayPalabra[a] = PALABRA.Substring(a, chunkSize);
            }

            label5.Text = cadena.Remove(cadena.Length - 3).ToUpper();
            //--------------------------------FIN SEPARAR PALABRA EN LETRAS---------------------------//


            //--------------------------------INICIO SEPARAR CLAVE EN LETRAS---------------------------//
            string CLAVE = txtclave.Text;
            string cadena2 = "";
            string new_clave = "";

            int LONGITUD_C = CLAVE.Length;
            int chunkSize2 = 1;

            String[] ArrayClave = new string[0];

            if (LONGITUD_C >= LONGITUD_P) //cuando la longitud de la clave es mayor igual a la longitud de la palabra
            {
                CLAVE = CLAVE.Substring(0, LONGITUD_P);

                int LenCadena = CLAVE.Length;
                ArrayClave = new string[LenCadena];

                for (int c = 0; c < LenCadena; c += chunkSize2)
                {
                    if (c + chunkSize2 > LenCadena) chunkSize2 = LenCadena - c;
                    cadena2 = cadena2 + (CLAVE.Substring(c, chunkSize2)) + " - ";

                    ArrayClave[c] = CLAVE.Substring(c, chunkSize2);
                }
            }
            else if (LONGITUD_C < LONGITUD_P)
            {
                int dif = LONGITUD_P - LONGITUD_C;
                int len = LONGITUD_C + dif;

                while (new_clave.Length <= len)
                {
                    new_clave += CLAVE;
                }

                new_clave = new_clave.Substring(0, len);
                ArrayClave = new string[len];

                for (int x = 0; x < len; x += chunkSize2)
                {
                    if (x + chunkSize2 > len) chunkSize2 = len - x;
                    cadena2 = cadena2 + (new_clave.Substring(x, chunkSize2)) + " - ";
                    ArrayClave[x] = new_clave.Substring(x, chunkSize2);
                }
            }

            label6.Text = cadena2.Remove(cadena2.Length - 3).ToUpper();
            //--------------------------------FIN SEPARAR CLAVE EN LETRAS---------------------------//

            //CREANDO LA TABLA DE LETRAS
            char letra = 'A';
            int i = 0;

            while (letra <= 'Z')
            {
                TablaLetras[i] = Convert.ToString(letra);
                TablaLetrasNumber[i] = Convert.ToString(i);
                i++;
                letra++;
            }
            //--------------------------

            //CREANDO LA TABLA DE NUMEROS
            int number = 0;
            int aux = 27;

            while (aux <=36)
            {
                TablaNumeros[number] = Convert.ToString(number);
                TablaNumerosNumber[number] = Convert.ToString(aux);
                number++;
                aux++;
            }
            //--------------------------


            //-----------------------INICIO DE CIFRADO-------------------------------------
            string searchLetra = "";
            string searchClave = "";
            string dato = "";
            string cifrado_palabra = "";
            string cifrado_clave = "";

            for (int b = 0; b < LONGITUD_P; b += chunkSize)
            {
                //CIFRANDO LA PALABRA-----------------------------------------------------
                searchLetra = ArrayPalabra[b];

                if (isNumeric(searchLetra))
                {
                    int index = Array.IndexOf(TablaNumeros, searchLetra);
                    dato = TablaNumerosNumber[index];
                }
                else {
                    int index = Array.IndexOf(TablaLetras, searchLetra);
                    dato = TablaLetrasNumber[index];
                }

                cifrado_palabra = cifrado_palabra + searchLetra + "="+ dato + " - ";
                //-----------------------------------------------------------------------



                //CIFRANDO LA CLAVE-----------------------------------------------------
                searchClave = ArrayClave[b];

                if (isNumeric(searchClave))
                {
                    int index = Array.IndexOf(TablaNumeros, searchClave);
                    dato = TablaNumerosNumber[index];
                }
                else
                {
                    int index = Array.IndexOf(TablaLetras, searchClave);
                    dato = TablaLetrasNumber[index];
                }

                cifrado_clave = cifrado_clave + searchClave + "=" + dato + " - ";
                //-----------------------------------------------------------------------
            }

            label7.Text = cifrado_palabra.Remove(cifrado_palabra.Length - 3).ToUpper().Replace("-"," ");

            label8.Text = cifrado_clave.Remove(cifrado_clave.Length - 3).ToUpper().Replace("-", " ");

            CifradoFinal(cifrado_palabra, cifrado_clave, LONGITUD_P);
            //-----------------------FIN DE CIFRADO-------------------------------------
        }

        public static Boolean isNumeric(String cadena)
        {
            Boolean resultado;

            try
            {
                int.Parse(cadena);
                resultado = true;
            }
            catch (Exception e)
            {
                resultado = false;
            }

            return resultado;
        }

        void CifradoFinal(string cifrado_palabra, string cifrado_clave, int longitud)
        {
            string[] palabra = cifrado_palabra.Split('-');
            string[] clave = cifrado_clave.Split('-');
            string[] dato;
            string[] dato2;
            int aux = 0;
            int aux2 = 0;
            int total = 0;
            string cifrado = "";
            string resultado = "";

            for (int i = 0; i < longitud; i++)
            {
                dato = palabra[i].Split('=');
                aux = Convert.ToInt32(dato[1]);

                dato2 = clave[i].Split('=');
                aux2 = Convert.ToInt32(dato2[1]);

                total = aux + aux2;

                if (total < 37)
                {
                    resultado = BuscarValor(total);
                }
                else {
                    total = total % 37;

                    resultado = BuscarValor(total);
                }

                cifrado = cifrado + resultado + "   ";
            }

            LBLCIFRADO.Text = cifrado.ToUpper();
        }

        string BuscarValor(int total)
        {
            string cifrado = "";
            int index = 0;

            if (total <= 25)
            {
                cifrado = TablaLetras[total];
            }
            else {
                index = Array.IndexOf(TablaNumerosNumber, Convert.ToString(total));
                cifrado = TablaNumeros[index];
            }

            return cifrado;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtpalabra.Clear();
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            LBLCIFRADO.Text = "";
        }
    }
}


