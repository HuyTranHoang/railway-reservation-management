import { Component } from '@angular/core';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss']
})
export class GalleryComponent {

  // https://www.freecodecamp.org/news/how-to-create-an-image-gallery-with-css-grid-e0f0fd666a5c/

  galleryImages = [
    { title: 'MUI NE', url: 'https://c0.wallpaperflare.com/preview/290/224/908/vietnam-phan-thiet-m%C5%A9i-ne-white-sand-dunes.jpg' },
    { title: 'HA GIANG', url: 'https://www.baolau.com/images/destinations/HGI.jpg' },
    { title: 'SAI GON', url: 'https://c0.wallpaperflare.com/preview/431/807/616/vietnam-landmark-81-ho-chi-minh-city-sky.jpg'},
    { title: 'NINH BINH', url: 'https://www.baolau.com/images/destinations/NBI-portrait.jpg' },
    { title: 'HA NOI', url: 'https://c0.wallpaperflare.com/preview/582/203/631/ha-noi-city-vietnam-lake.jpg'},
    { title: 'CAO BANG', url: 'https://www.baolau.com/images/destinations/VNCAB.jpg'},
    { title: 'HOI AN', url: 'https://c0.wallpaperflare.com/preview/400/148/836/vietnam-h%E1%BB%99i-an-ancient-town-street.jpg'},
    { title: 'NHA TRANG', url: 'https://c4.wallpaperflare.com/wallpaper/470/673/182/nha-trang-beach-tour-1-wallpaper-preview.jpg'},
    { title: 'HA LONG', url: 'https://c4.wallpaperflare.com/wallpaper/515/503/662/photography-h%E1%BA%A1-long-bay-boat-earth-wallpaper-preview.jpg'}
  ];

}
