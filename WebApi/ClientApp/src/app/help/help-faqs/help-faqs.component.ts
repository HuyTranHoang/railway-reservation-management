import { Component } from '@angular/core';

@Component({
  selector: 'app-help-faqs',
  templateUrl: './help-faqs.component.html',
  styleUrls: ['./help-faqs.component.scss']
})
export class HelpFaqsComponent {

  isPanelOpen: boolean[] = Array(10).fill(false);

  constructor() { }

  togglePanel($event: any, index: number, ) {
    this.isPanelOpen[index] = $event;
  }

}
