using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;  //フォームがドラッグアンドドロップを受けられるようにする

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }
        private void xMakeZip(string[] srcfiles, string dst_zip_file)
        {
            //ZipFileを作成する
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                //IBM437でエンコードできないファイル名やコメントをShift JISでエンコード
                zip.ProvisionalAlternateEncoding =
                    System.Text.Encoding.GetEncoding("shift_jis");
                //IBM437でエンコードできないファイル名やコメントをUTF-8でエンコード
                //zip.UseUnicodeAsNecessary = true;
                //圧縮レベルを変更
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                //圧縮せずに格納する
                //zip.ForceNoCompression = true;
                //必要な時はZIP64で圧縮する。デフォルトはNever。
                zip.UseZip64WhenSaving = Ionic.Zip.Zip64Option.AsNecessary;
                //エラーが出てもスキップする。デフォルトはThrow。
                zip.ZipErrorAction = Ionic.Zip.ZipErrorAction.Skip;
             
                //ZIP書庫にコメントを付ける
                //zip.Comment = "こんにちは。";
                for (int i = 0; i < srcfiles.Length; i++)
                {
                    string fn = srcfiles[i];
                    //ファイルを追加する
                    zip.AddFile(fn,".");
                }
                //ZIP書庫を作成する
                zip.Save(dst_zip_file);
            }
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            //コントロール内にドロップされたとき実行される
            //ドロップされたすべてのファイル名を取得する
            string[] fileNames =(string[])e.Data.GetData(DataFormats.FileDrop, false);
            //ListBoxに追加する
            //listBox1.Items.AddRange(fileName);

            xMakeZip(fileNames, "test.zip");

        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            //コントロール内にドラッグされたとき実行される
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                //ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
                e.Effect = DragDropEffects.Copy;
            else
                //ファイル以外は受け付けない
                e.Effect = DragDropEffects.None;
        }
        private void xLog(String s)
        {
            //ログ出力
            DateTime dt = DateTime.Now;
            textBox1.AppendText(dt.ToString() + " " + s + "\n");
            System.IO.StreamWriter sw = new System.IO.StreamWriter(@"log.txt", true, System.Text.Encoding.GetEncoding("shift_jis"));
            sw.Write(dt.ToString() + " " + s + "\r\n");
            sw.Close();
        }
    }
}
