import { Component } from '@angular/core';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-user',
  template: `
    <router-outlet></router-outlet>
  `,
})
export class UserComponent {

  constructor(public authService: AuthService) {}

}
