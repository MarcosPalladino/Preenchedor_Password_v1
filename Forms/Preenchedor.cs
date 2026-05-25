using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPPreenchedor.Data;
using TPPreenchedor.Data.Models;

namespace TPPreenchedor.Forms
{
    public class Preenchedor : Form
    {
        private struct INPUT
        {
            public int type;

            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

#pragma warning disable 0649
        private struct MOUSEINPUT
        {
            public int dx;

            public int dy;

            public int mouseData;

            public int dwFlags;

            public int time;

            public IntPtr dwExtraInfo;
        }

        private struct KEYBDINPUT
        {
            public short wVk;

            public short wScan;

            public int dwFlags;

            public int time;

            public IntPtr dwExtraInfo;
        }

        private struct HARDWAREINPUT
        {
            public int uMsg;

            public short wParamL;

            public short wParamH;
        }
#pragma warning restore 0649

        private const int INPUT_KEYBOARD = 1;

        private const int KEYEVENTF_UNICODE = 4;

        private const int KEYEVENTF_KEYUP = 2;

        private IContainer components = null;

        private TextBox txtDadosPreencherUsrTpb;

        private Button btnPreencherUsrTpb;

        private TrackBar trackBar;

        private Label label1;
        private GroupBox groupBoxTpb;
        private GroupBox groupBox1;
        private TextBox txtDadosPreencherUsrTpbAdm1;
        private Label label3;
        private Button btnPreencherUsrTpbAdm1;
        private GroupBox groupBox2;
        private TextBox txtDadosPreencherUsrTpbAdm2;
        private Label label4;
        private Button btnPreencherUsrTpbAdm2;
        private GroupBox groupBox3;
        private TextBox txtDadosPreencherUsrTpItau;
        private Label label5;
        private Button btnPreencherUsrTpItau;
        private GroupBox groupBox4;
        private TextBox txtDadosPreencherUsrUol;
        private Label label6;
        private Button btnPreencherUsrUol;
        private Label label2;

        private const string LoginUsrTpb = @"tpb\palladino.11";
        private const string LoginUsrTpbAdm1 = @"tpb\palladino.11-adm1";
        private const string LoginUsrTpbAdm2 = @"tpb\palladino.11-adm2";
        private const string LoginUsrTpItau = @"tpitau\palladino.11";
        private const string LoginUsrUol = @"uol\palladino.11.uol";

        public Preenchedor()
        {
            InitializeComponent();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = "TEMPO DE ESPERA (" + trackBar.Value + " SEG)";
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        private async void btnPreencherUsrTpb_Click(object sender, EventArgs e)
        {
            await PreencherTextoAsync(txtDadosPreencherUsrTpb, "USR TPB");
        }

        private async Task PreencherTextoAsync(TextBox textBox, string descricao)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                MessageBox.Show("Voce precisa informar os dados para preencher " + descricao);
                return;
            }

            try
            {
                SetControlesPreenchimentoEnabled(false);
                await Task.Delay(trackBar.Value * 1000);

                foreach (char c in textBox.Text)
                {
                    SendUnicodeChar(c);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(descricao + " - Ocorreu um erro inesperado: " + ex.Message);
            }
            finally
            {
                SetControlesPreenchimentoEnabled(true);
            }
        }

        private void SetControlesPreenchimentoEnabled(bool enabled)
        {
            trackBar.Enabled = enabled;
            txtDadosPreencherUsrTpb.Enabled = enabled;
            txtDadosPreencherUsrTpbAdm1.Enabled = enabled;
            txtDadosPreencherUsrTpbAdm2.Enabled = enabled;
            txtDadosPreencherUsrTpItau.Enabled = enabled;
            txtDadosPreencherUsrUol.Enabled = enabled;
            btnPreencherUsrTpb.Enabled = enabled;
            btnPreencherUsrTpbAdm1.Enabled = enabled;
            btnPreencherUsrTpbAdm2.Enabled = enabled;
            btnPreencherUsrTpItau.Enabled = enabled;
            btnPreencherUsrUol.Enabled = enabled;
        }

        private void SendUnicodeChar(char c)
        {
            INPUT[] array = new INPUT[1];
            array[0].type = 1;
            array[0].u.ki.wVk = 0;
            array[0].u.ki.wScan = (short)c;
            array[0].u.ki.dwFlags = 4;
            array[0].u.ki.time = 0;
            array[0].u.ki.dwExtraInfo = IntPtr.Zero;
            if (SendInput(1u, array, Marshal.SizeOf((object)array[0])) == 0)
            {
                throw new Exception("Erro ao enviar entrada de teclado");
            }
            array[0].u.ki.dwFlags |= 2;
            if (SendInput(1u, array, Marshal.SizeOf((object)array[0])) == 0)
            {
                throw new Exception("Erro ao enviar entrada de teclado");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Preenchedor));
            this.txtDadosPreencherUsrTpb = new System.Windows.Forms.TextBox();
            this.btnPreencherUsrTpb = new System.Windows.Forms.Button();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxTpb = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDadosPreencherUsrTpbAdm1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnPreencherUsrTpbAdm1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtDadosPreencherUsrTpbAdm2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPreencherUsrTpbAdm2 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtDadosPreencherUsrTpItau = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnPreencherUsrTpItau = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtDadosPreencherUsrUol = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnPreencherUsrUol = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.groupBoxTpb.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDadosPreencherUsrTpb
            // 
            this.txtDadosPreencherUsrTpb.Location = new System.Drawing.Point(160, 18);
            this.txtDadosPreencherUsrTpb.Name = "txtDadosPreencherUsrTpb";
            this.txtDadosPreencherUsrTpb.Size = new System.Drawing.Size(180, 20);
            this.txtDadosPreencherUsrTpb.TabIndex = 0;
            // 
            // btnPreencherUsrTpb
            // 
            this.btnPreencherUsrTpb.Location = new System.Drawing.Point(368, 13);
            this.btnPreencherUsrTpb.Name = "btnPreencherUsrTpb";
            this.btnPreencherUsrTpb.Size = new System.Drawing.Size(97, 31);
            this.btnPreencherUsrTpb.TabIndex = 1;
            this.btnPreencherUsrTpb.Text = "PREENCHER";
            this.btnPreencherUsrTpb.UseVisualStyleBackColor = true;
            this.btnPreencherUsrTpb.Click += new System.EventHandler(this.btnPreencherUsrTpb_Click);
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(190, 10);
            this.trackBar.Minimum = 3;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(180, 45);
            this.trackBar.TabIndex = 2;
            this.trackBar.Value = 3;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "TEMPO DE ESPERA (3 SEG)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "PWD USUARIO TPB";
            // 
            // groupBoxTpb
            // 
            this.groupBoxTpb.Controls.Add(this.txtDadosPreencherUsrTpb);
            this.groupBoxTpb.Controls.Add(this.label2);
            this.groupBoxTpb.Controls.Add(this.btnPreencherUsrTpb);
            this.groupBoxTpb.Location = new System.Drawing.Point(11, 61);
            this.groupBoxTpb.Name = "groupBoxTpb";
            this.groupBoxTpb.Size = new System.Drawing.Size(504, 57);
            this.groupBoxTpb.TabIndex = 5;
            this.groupBoxTpb.TabStop = false;
            this.groupBoxTpb.Text = "Usr Tpb";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtDadosPreencherUsrTpbAdm1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnPreencherUsrTpbAdm1);
            this.groupBox1.Location = new System.Drawing.Point(12, 124);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(504, 57);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Usr Tpb Adm1";
            // 
            // txtDadosPreencherUsrTpbAdm1
            // 
            this.txtDadosPreencherUsrTpbAdm1.Location = new System.Drawing.Point(160, 18);
            this.txtDadosPreencherUsrTpbAdm1.Name = "txtDadosPreencherUsrTpbAdm1";
            this.txtDadosPreencherUsrTpbAdm1.Size = new System.Drawing.Size(180, 20);
            this.txtDadosPreencherUsrTpbAdm1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "PWD USUARIO ADM1";
            // 
            // btnPreencherUsrTpbAdm1
            // 
            this.btnPreencherUsrTpbAdm1.Location = new System.Drawing.Point(368, 13);
            this.btnPreencherUsrTpbAdm1.Name = "btnPreencherUsrTpbAdm1";
            this.btnPreencherUsrTpbAdm1.Size = new System.Drawing.Size(97, 31);
            this.btnPreencherUsrTpbAdm1.TabIndex = 1;
            this.btnPreencherUsrTpbAdm1.Text = "PREENCHER";
            this.btnPreencherUsrTpbAdm1.UseVisualStyleBackColor = true;
            this.btnPreencherUsrTpbAdm1.Click += new System.EventHandler(this.btnPreencherUsrTpbAdm1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtDadosPreencherUsrTpbAdm2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnPreencherUsrTpbAdm2);
            this.groupBox2.Location = new System.Drawing.Point(11, 187);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(504, 57);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Usr Tpb Adm2";
            // 
            // txtDadosPreencherUsrTpbAdm2
            // 
            this.txtDadosPreencherUsrTpbAdm2.Location = new System.Drawing.Point(160, 18);
            this.txtDadosPreencherUsrTpbAdm2.Name = "txtDadosPreencherUsrTpbAdm2";
            this.txtDadosPreencherUsrTpbAdm2.Size = new System.Drawing.Size(180, 20);
            this.txtDadosPreencherUsrTpbAdm2.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "PWD USUARIO ADM2";
            // 
            // btnPreencherUsrTpbAdm2
            // 
            this.btnPreencherUsrTpbAdm2.Location = new System.Drawing.Point(368, 13);
            this.btnPreencherUsrTpbAdm2.Name = "btnPreencherUsrTpbAdm2";
            this.btnPreencherUsrTpbAdm2.Size = new System.Drawing.Size(97, 31);
            this.btnPreencherUsrTpbAdm2.TabIndex = 1;
            this.btnPreencherUsrTpbAdm2.Text = "PREENCHER";
            this.btnPreencherUsrTpbAdm2.UseVisualStyleBackColor = true;
            this.btnPreencherUsrTpbAdm2.Click += new System.EventHandler(this.btnPreencherUsrTpbAdm2_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtDadosPreencherUsrTpItau);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.btnPreencherUsrTpItau);
            this.groupBox3.Location = new System.Drawing.Point(12, 250);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(504, 57);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Usr Tpitau";
            // 
            // txtDadosPreencherUsrTpItau
            // 
            this.txtDadosPreencherUsrTpItau.Location = new System.Drawing.Point(160, 18);
            this.txtDadosPreencherUsrTpItau.Name = "txtDadosPreencherUsrTpItau";
            this.txtDadosPreencherUsrTpItau.Size = new System.Drawing.Size(180, 20);
            this.txtDadosPreencherUsrTpItau.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "PWD USUARIO ITAU";
            // 
            // btnPreencherUsrTpItau
            // 
            this.btnPreencherUsrTpItau.Location = new System.Drawing.Point(368, 13);
            this.btnPreencherUsrTpItau.Name = "btnPreencherUsrTpItau";
            this.btnPreencherUsrTpItau.Size = new System.Drawing.Size(97, 31);
            this.btnPreencherUsrTpItau.TabIndex = 1;
            this.btnPreencherUsrTpItau.Text = "PREENCHER";
            this.btnPreencherUsrTpItau.UseVisualStyleBackColor = true;
            this.btnPreencherUsrTpItau.Click += new System.EventHandler(this.btnPreencherUsrTpItau_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtDadosPreencherUsrUol);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.btnPreencherUsrUol);
            this.groupBox4.Location = new System.Drawing.Point(11, 313);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(504, 57);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Usr Uol";
            // 
            // txtDadosPreencherUsrUol
            // 
            this.txtDadosPreencherUsrUol.Location = new System.Drawing.Point(160, 18);
            this.txtDadosPreencherUsrUol.Name = "txtDadosPreencherUsrUol";
            this.txtDadosPreencherUsrUol.Size = new System.Drawing.Size(180, 20);
            this.txtDadosPreencherUsrUol.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "PWD USUARIO UOL";
            // 
            // btnPreencherUsrUol
            // 
            this.btnPreencherUsrUol.Location = new System.Drawing.Point(368, 13);
            this.btnPreencherUsrUol.Name = "btnPreencherUsrUol";
            this.btnPreencherUsrUol.Size = new System.Drawing.Size(97, 31);
            this.btnPreencherUsrUol.TabIndex = 1;
            this.btnPreencherUsrUol.Text = "PREENCHER";
            this.btnPreencherUsrUol.UseVisualStyleBackColor = true;
            this.btnPreencherUsrUol.Click += new System.EventHandler(this.btnPreencherUsrUol_Click);
            // 
            // Preenchedor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 374);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxTpb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Preenchedor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preenchedor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Preenchedor_FormClosing);
            this.Load += new System.EventHandler(this.Preenchedor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.groupBoxTpb.ResumeLayout(false);
            this.groupBoxTpb.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private async void btnPreencherUsrTpbAdm1_Click(object sender, EventArgs e)
        {
            await PreencherTextoAsync(txtDadosPreencherUsrTpbAdm1, "USR ADM1");
        }

        private async void btnPreencherUsrTpbAdm2_Click(object sender, EventArgs e)
        {
            await PreencherTextoAsync(txtDadosPreencherUsrTpbAdm2, "USR ADM2");
        }

        private async void btnPreencherUsrTpItau_Click(object sender, EventArgs e)
        {
            await PreencherTextoAsync(txtDadosPreencherUsrTpItau, "USR ITAU");
        }

        private async void btnPreencherUsrUol_Click(object sender, EventArgs e)
        {
            await PreencherTextoAsync(txtDadosPreencherUsrUol, "USR UOL");
        }

        private void Preenchedor_Load(object sender, EventArgs e)
        {
            try
            {
                using (var usuarioRepo = new UsuarioRepository())
                {
                    var campos = ObterCamposCredenciais();
                    var senhasPorLogin = usuarioRepo.ObterSenhasPorLogin(campos.Keys);

                    foreach (var campo in campos)
                    {
                        string senha;

                        if (senhasPorLogin.TryGetValue(campo.Key, out senha))
                        {
                            campo.Value.Text = senha;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao carregar as credenciais: " + ex.Message);
            }
        }

        private void Preenchedor_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                using (var usuarioRepo = new UsuarioRepository())
                {
                    usuarioRepo.SalvarUsuarios(ObterUsuariosDaTela());
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                MessageBox.Show("Ocorreu um erro ao salvar as credenciais: " + ex.Message);
            }
        }

        private Dictionary<string, TextBox> ObterCamposCredenciais()
        {
            return new Dictionary<string, TextBox>
            {
                { LoginUsrTpb, txtDadosPreencherUsrTpb },
                { LoginUsrTpbAdm1, txtDadosPreencherUsrTpbAdm1 },
                { LoginUsrTpbAdm2, txtDadosPreencherUsrTpbAdm2 },
                { LoginUsrTpItau, txtDadosPreencherUsrTpItau },
                { LoginUsrUol, txtDadosPreencherUsrUol }
            };
        }

        private IEnumerable<Usuario> ObterUsuariosDaTela()
        {
            foreach (var campo in ObterCamposCredenciais())
            {
                yield return new Usuario
                {
                    Login = campo.Key,
                    Senha = campo.Value.Text.Trim()
                };
            }
        }
    }
}
