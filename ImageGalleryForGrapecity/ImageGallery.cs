using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using C1.Win.C1Tile;


namespace ImageGalleryForGrapecity
{
	public partial class ImageGallery : Form
	{
		DataFetcher datafetch = new DataFetcher();
		List<ImageItem> imageList;
		int checkedItems = 0;
		C1.C1Pdf.C1PdfDocument imagePdfDocument = new C1.C1Pdf.C1PdfDocument();
		public ImageGallery()
		{
			InitializeComponent();
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private async void _search_Click(object sender, EventArgs e)
		{
			statusStrip1.Visible = true;
			imageList = await datafetch.GetImageData(_searchBox.Text);
			AddTiles(imageList);
			statusStrip1.Visible = false;
		}
		private void AddTiles(List<ImageItem> imageList)
		{
			_imageTileControl.Groups[0].Tiles.Clear();

			foreach (var imageitem in imageList)
			{
				Tile tile = new Tile();
				tile.HorizontalSize = 2;
				tile.VerticalSize = 2;

				_imageTileControl.Groups[0].Tiles.Add(tile);
				Image img = Image.FromStream(new MemoryStream(imageitem.Base64));

				Template t1 = new Template();
				ImageElement ie = new ImageElement();
				ie.ImageLayout = ForeImageLayout.Stretch;
				t1.Elements.Add(ie);
				tile.Template = t1;
				tile.Image = img;

			}
		}

		private void _exportimage_Click(object sender, EventArgs e)
		{
			List<Image> images = new List<Image>();
			foreach (Tile tile in _imageTileControl.Groups[0].Tiles)
			{
				if (tile.Checked)
				{
					images.Add(tile.Image);
				}
			}
			ConvertToPdf(images);
			SaveFileDialog saveFile =new SaveFileDialog();
			saveFile.DefaultExt = "pdf";
			saveFile.Filter = "PDF files (*.pdf)|*.pdf*";

			if (saveFile.ShowDialog() == DialogResult.OK)
			{
				imagePdfDocument.Save(saveFile.FileName);

			}
		}

		private void ConvertToPdf(List<Image> images)
		{
			RectangleF rect = imagePdfDocument.PageRectangle;
			bool firstPage = true;
			foreach (var selectedimg in images)
			{
				if (!firstPage)
				{
					imagePdfDocument.NewPage();
				}
				firstPage = false;
				rect.Inflate(-72, -72);
				imagePdfDocument.DrawImage(selectedimg, rect);
			}
		}


		private void OnSearchPanelPaint(object sender, PaintEventArgs e)
		{
			Rectangle r = _searchBox.Bounds;
			r.Inflate(3, 3);
			Pen p = new Pen(Color.LightGray);
			e.Graphics.DrawRectangle(p, r);
		}


		private void _exportimage_Paint(object sender, PaintEventArgs e)
		{
			Rectangle r = new Rectangle(_exportimage.Location.X, _exportimage.Location.Y, _exportimage.Width, _exportimage.Height);
			r.X -= 29;
			r.Y -= 3;
			r.Width--;
			r.Height--;
			Pen p = new Pen(Color.LightGray);
			e.Graphics.DrawRectangle(p, r);
			e.Graphics.DrawLine(p, new Point(0, 43), new Point(this.Width, 43));
		}

		private void _imageTileControl_Paint(object sender, PaintEventArgs e)
		{
			Pen p = new Pen(Color.LightGray);
			e.Graphics.DrawLine(p, 0, 43, 800, 43);
		}

		private void _imageTileControl_TileChecked(object sender, C1.Win.C1Tile.TileEventArgs e)
		{
			checkedItems++;

			_exportimage.Visible = true;
		}
	

		private void _imageTileControl_TileUnchecked(object sender, C1.Win.C1Tile.TileEventArgs e)
		{
			checkedItems--;
			_exportimage.Visible = checkedItems > 0;
		}

		private void ImageGallery_Load(object sender, EventArgs e)
		{

		}

        private void tile1_Click(object sender, EventArgs e)
        {

        }

        private void _exportimage_Click_1(object sender, EventArgs e)
        {
			List<Image> images = new List<Image>();
			foreach (Tile tile in _imageTileControl.Groups[0].Tiles)
			{
				if (tile.Checked)
				{
					images.Add(tile.Image);
				}
			}
			ConvertToPdf(images);
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.DefaultExt = "pdf";
			saveFile.Filter = "PDF files (*.pdf)|*.pdf*";

			if (saveFile.ShowDialog() == DialogResult.OK)
			{
				imagePdfDocument.Save(saveFile.FileName);

			}
		}
    }
}
