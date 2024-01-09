import { Component } from '@angular/core';

@Component({
  selector: 'ngx-footer',
  styleUrls: ['./footer.component.scss'],
  template: `
    <span class="created-by">
      Created with ♥ by <b><a href="https://github.com/HuyTranHoang/railway-reservation-management" target="_blank">Group 2</a></b> 2024
    </span>
    <div class="socials">
      <a href="https://github.com/HuyTranHoang/railway-reservation-management" target="_blank" class="ion ion-social-github"></a>
    </div>
  `,
})
export class FooterComponent {
}
