import { AfterViewInit, Component, Inject, Renderer2 } from '@angular/core'
import { DOCUMENT } from '@angular/common'

@Component({
  selector: 'app-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.scss']
})
export class SliderComponent implements AfterViewInit {

  constructor(private renderer2: Renderer2,
              @Inject(DOCUMENT) private document: Document,) {}

  sliderImages = [
    'assets/slider/train-japan.jpg',
    'assets/slider/train-thailand.jpg',
    'assets/slider/train-vietnam.jpg',
  ]

  ngAfterViewInit() {
    const script = this.renderer2.createElement('script')
    script.src = 'https://cdn.jsdelivr.net/npm/swiper@11/swiper-element-bundle.min.js'
    script.defer = true
    this.renderer2.appendChild(this.document.body, script)
  }

}
