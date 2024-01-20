import { Component } from '@angular/core';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss']
})
export class GalleryComponent {

  // https://www.freecodecamp.org/news/how-to-create-an-image-gallery-with-css-grid-e0f0fd666a5c/

  galleryImages = [
    { title: 'FUJI', url: 'https://www.baolau.com/images/destinations/JPFJI.jpg' },
    { title: 'HA GIANG', url: 'https://www.baolau.com/images/destinations/HGI.jpg' },
    { title: 'LANGKAWI', url: 'https://www.baolau.com/images/destinations/LGK-portrait.jpg'},
    { title: 'NINH BINH', url: 'https://www.baolau.com/images/destinations/NBI-portrait.jpg' },
    { title: 'KHAO YAI', url: 'https://www.baolau.com/images/destinations/PCH.jpg'},
    { title: 'CAO BANG', url: 'https://www.baolau.com/images/destinations/VNCAB.jpg'},
    { title: 'KHAO SOK', url: 'https://www.baolau.com/images/destinations/KSK-portrait.jpg'},
    { title: 'LOMBOK', url: 'https://www.baolau.com/images/destinations/LOP.jpg'},
    { title: 'BOHOL', url: 'https://www.baolau.com/images/destinations/TAG.jpg'}
  ];

}
