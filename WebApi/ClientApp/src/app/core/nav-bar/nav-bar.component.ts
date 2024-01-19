import { Component, HostListener, OnInit } from '@angular/core'
import { AuthService } from '../../auth/auth.service'
import { NavigationEnd, Router } from '@angular/router'

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  href: string = '';
  isScroll = false;

  specialRoutes = [ // Các route không cần hiển thị nav-bar trong suốt
    '/auth/login',
    '/auth/register',
    '/auth/forgot-password',
    '/test-error',
    '/not-found',
    '/booking',
  ];

  constructor(public authService: AuthService,
              private router: Router) {}

  ngOnInit(): void {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.href = this.router.url;
        console.log(this.router.url);
      }
    });
  }

  checkStartsWith(href: string): boolean {
    return this.specialRoutes.some(route => href.startsWith(route));
  }

  @HostListener('window:scroll', ['$event'])
  onWindowScroll(event: any) {
    // Thực hiện các tác vụ khi sự kiện scroll xảy ra
    this.checkScroll();
  }

  checkScroll() {
    const scrollPosition = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
    this.isScroll = scrollPosition >= 300;
  }

  logout() {
    this.authService.logout();
  }
}
