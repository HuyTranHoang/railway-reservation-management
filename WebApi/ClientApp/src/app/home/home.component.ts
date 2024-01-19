import { AfterViewInit, Component, Inject, Renderer2 } from '@angular/core'
import { DOCUMENT } from '@angular/common'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements AfterViewInit {

  sliderImages = [
    'assets/slider/train-japan.jpg',
    'assets/slider/train-thailand.jpg',
    'assets/slider/train-vietnam.jpg',
  ]

  constructor( private renderer2: Renderer2,
               @Inject(DOCUMENT) private document: Document,) {
  }

  ngAfterViewInit() {
    const script = this.renderer2.createElement('script')
    script.src = 'https://cdn.jsdelivr.net/npm/swiper@11/swiper-element-bundle.min.js'
    script.defer = true
    this.renderer2.appendChild(this.document.body, script)
  }
}
