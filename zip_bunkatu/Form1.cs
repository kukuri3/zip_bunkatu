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
        private void xMakeZip(List<string> srcfiles,string dstfile)
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
             

                for (int i = 0; i < srcfiles.Count; i++)
                {
                    string fn = srcfiles[i];
                    //ファイルを追加する
                    zip.AddFile(fn,".");

                }
                //zipを作る
                zip.Save(dstfile);
            }
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            //コントロール内にドロップされたとき実行される
            //ドロップされたすべてのファイル名を取得する
            string[] fileNames =(string[])e.Data.GetData(DataFormats.FileDrop, false);
            long nowsize = 0;
            long nowfileno = 0;
            long maxsize=10000;

            xLog(fileNames.Length + "files.");

            var fnlist = new List<string>();    //今回zipするファイル名のリスト

            for (int i = 0; i < fileNames.Length; i++)
            {
                fnlist.Add(fileNames[i]);   //リストに追加
                System.IO.FileInfo fi = new System.IO.FileInfo(fileNames[i]);
                nowsize += fi.Length;
                if (nowsize > maxsize)
                {
                    string zipfn = nowfileno.ToString() + ".zip";
                    xMakeZip(fnlist,zipfn);
                    nowsize = 0;
                    nowfileno++;
                    fnlist.Clear();

                }
            }
            //残りがあればさいごにzipする
            if (fnlist.Count > 0)
            {
                string zipfn = nowfileno.ToString() + ".zip";
                xMakeZip(fnlist, zipfn);
            }
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
