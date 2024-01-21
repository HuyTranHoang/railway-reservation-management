import { Component } from '@angular/core'

@Component({
  selector: 'app-faqs',
  templateUrl: './faqs.component.html',
  styleUrls: ['./faqs.component.scss']
})
export class FaqsComponent {

  isPanelOpen: boolean[] = Array(10).fill(false);

  constructor() { }

  togglePanel($event: any, index: number, ) {
    this.isPanelOpen[index] = $event;
  }

}
