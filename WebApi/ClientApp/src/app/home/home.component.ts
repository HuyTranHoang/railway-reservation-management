import {Component} from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent  {
  myInterval = 1500;
  activeSlideIndex = 0;
  slides: {image: string; text?: string}[] = [
    {image: 'assets/images/nature/3.jpg'},
    {image: 'assets/images/nature/2.jpg'},
    {image: 'assets/images/nature/1.jpg'}
  ];
}
