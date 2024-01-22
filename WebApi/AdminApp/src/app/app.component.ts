/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import {Component, OnInit} from '@angular/core';
import {AnalyticsService} from './@core/utils';
import {SeoService} from './@core/utils';
import {AuthService} from './auth/auth.service';

@Component({
  selector: 'ngx-app',
  template: `
    <nb-layout>
      <nb-layout-column style="padding: 0">
        <router-outlet></router-outlet>
      </nb-layout-column>
    </nb-layout>`,
})
export class AppComponent implements OnInit {

  constructor(private analytics: AnalyticsService,
              private authService: AuthService,
              private seoService: SeoService) {
  }

  ngOnInit(): void {
    this.analytics.trackPageViews();
    this.seoService.trackCanonicalChanges();
    this.refreshUser();
  }

  private refreshUser() {
    const jwt = this.authService.getJwtToken();

    if (jwt) {
      this.authService.refreshUser(jwt).subscribe({
        next: () => console.log('User refreshed'),
        error: (err) => {
          this.authService.logout();
          console.log(err);
        },
      });
    } else {
      this.authService.refreshUser(null).subscribe();
    }
  }
}
