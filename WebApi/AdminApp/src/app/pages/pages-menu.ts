import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS: NbMenuItem[] = [
  {
    title: 'E-commerce',
    icon: 'shopping-cart-outline',
    link: '/pages/dashboard',
    home: true,
  },
  {
    title: 'IoT Dashboard',
    icon: 'home-outline',
    link: '/pages/iot-dashboard',
  },
  {
    title: 'FEATURES',
    group: true,
  },
  {
    title: 'Layout',
    icon: 'layout-outline',
    children: [
      {
        title: 'Stepper',
        link: '/pages/layout/stepper',
      },
      {
        title: 'List',
        link: '/pages/layout/list',
      },
      {
        title: 'Infinite List',
        link: '/pages/layout/infinite-list',
      },
      {
        title: 'Accordion',
        link: '/pages/layout/accordion',
      },
      {
        title: 'Tabs',
        pathMatch: 'prefix',
        link: '/pages/layout/tabs',
      },
    ],
  },
  {
    title: 'Forms',
    icon: 'edit-2-outline',
    children: [
      {
        title: 'Form Inputs',
        link: '/pages/forms/inputs',
      },
      {
        title: 'Form Layouts',
        link: '/pages/forms/layouts',
      },
      {
        title: 'Buttons',
        link: '/pages/forms/buttons',
      },
      {
        title: 'Datepicker',
        link: '/pages/forms/datepicker',
      },
    ],
  },
  {
    title: 'UI Features',
    icon: 'keypad-outline',
    link: '/pages/ui-features',
    children: [
      {
        title: 'Grid',
        link: '/pages/ui-features/grid',
      },
      {
        title: 'Icons',
        link: '/pages/ui-features/icons',
      },
      {
        title: 'Typography',
        link: '/pages/ui-features/typography',
      },
      {
        title: 'Animated Searches',
        link: '/pages/ui-features/search-fields',
      },
    ],
  },
  {
    title: 'Modal & Overlays',
    icon: 'browser-outline',
    children: [
      {
        title: 'Dialog',
        link: '/pages/modal-overlays/dialog',
      },
      {
        title: 'Window',
        link: '/pages/modal-overlays/window',
      },
      {
        title: 'Popover',
        link: '/pages/modal-overlays/popover',
      },
      {
        title: 'Toastr',
        link: '/pages/modal-overlays/toastr',
      },
      {
        title: 'Tooltip',
        link: '/pages/modal-overlays/tooltip',
      },
    ],
  },
  {
    title: 'Extra Components',
    icon: 'message-circle-outline',
    children: [
      {
        title: 'Calendar',
        link: '/pages/extra-components/calendar',
      },
      {
        title: 'Progress Bar',
        link: '/pages/extra-components/progress-bar',
      },
      {
        title: 'Spinner',
        link: '/pages/extra-components/spinner',
      },
      {
        title: 'Alert',
        link: '/pages/extra-components/alert',
      },
      {
        title: 'Calendar Kit',
        link: '/pages/extra-components/calendar-kit',
      },
      {
        title: 'Chat',
        link: '/pages/extra-components/chat',
      },
    ],
  },
  {
    title: 'Maps',
    icon: 'map-outline',
    children: [
      {
        title: 'Google Maps',
        link: '/pages/maps/gmaps',
      },
      {
        title: 'Leaflet Maps',
        link: '/pages/maps/leaflet',
      },
      {
        title: 'Bubble Maps',
        link: '/pages/maps/bubble',
      },
      {
        title: 'Search Maps',
        link: '/pages/maps/searchmap',
      },
    ],
  },
  {
    title: 'Charts',
    icon: 'pie-chart-outline',
    children: [
      {
        title: 'Echarts',
        link: '/pages/charts/echarts',
      },
      {
        title: 'Charts.js',
        link: '/pages/charts/chartjs',
      },
      {
        title: 'D3',
        link: '/pages/charts/d3',
      },
    ],
  },
      {

        title: 'Railway Management',
        icon: 'message-square-outline',
        link: '/pages/management/trainmanagement',
        children :[
          {
            title: 'Train Station',
            link: '/pages/railway/trainstation',
          },
          {
            title: 'Train Company',
            link: '/pages/railway/traincompany',
          },
        ]
      },  
      
      {
        title: 'Passenger & Ticket', 
        icon: 'award-outline',
        link: '/pages/management/passenger-ticket',
        children :[
          {
            title: 'Passenger',
            link: '/pages/passenger-ticket/passenger',
          },
          {
            title: 'Ticket',
            link: '/pages/passenger-ticket/ticket',
          },
        ]
      },  
      {
        title: 'Schedule & Ticket Price',
        icon: 'calendar-outline',
        link: '/pages/management/schedule-ticketprice',
        children :[
          {
            title: 'Schedule',
            link: '/pages/schedule-ticketprice/schedule',
          },
          {
            title: 'Ticket Price',
            link: '/pages/schedule-ticketprice/ticketprice',
          },
        ]
      },  
      {
        title: 'Seat & Seat Type',
        icon : 'shopping-bag-outline',
        link: '/pages/management/seat-seatype',
        children :[
          {
            title: 'Seat',
            link: '/pages/seat-seattype/seat',
          },
          {
            title: 'Seat Type',
            link: '/pages/seat-seattype/seattype',
          },
        ]
      },  
      {
        title: 'Train & Carriage',
        icon : 'swap-outline',
        link: '/pages/management/train-carriage',
        children :[
          {
            title: 'Train',
            link: '/pages/train-carriage/train',
          },
          {
            title: 'Carriage',
            link: '/pages/train-carriage/carriage',
          },
        ]
      },  
      {
        title: 'Payment & Cancellation',
        icon : 'paper-plane-outline',
        link: '/pages/management/payment-cancellation',
        children :[
          {
            title: 'Payment',
            link: '/pages/payment-cancellation/payment',
          },
          {
            title: 'Cancellation',
            link: '/pages/payment-cancellation/cancellation',
          },
        ]
      },  
      {
        title: 'System Management',
        icon : 'shield-outline',        
        link: '/pages/system',
      },  
      {
        title: 'Report & Statitics',
        icon : 'pantone-outline',    
        link: '/pages/management/report-statitics',
        children :[
          {
            title: 'Report',
            link: '/pages/report-statitics/report',
          },
          {
            title: 'Statitics',
            link: '/pages/report-statitics/statitics',
          },
        ]
      },  
        title: 'Ticket Management',
        link: '/pages/management/ticketmanagement',
      },
      {
        title: 'Schedule Management',
        link: '/pages/management/schedulemanagement',
      },
      {
        title: 'Train Management',
        link: '/pages/management/trainmanagement',
      },
    ],
  },

  {
    title: 'Editors',
    icon: 'text-outline',
    children: [
      {
        title: 'TinyMCE',
        link: '/pages/editors/tinymce',
      },
    ],
  },
  {
    title: 'Tables & Data',
    icon: 'grid-outline',
    children: [
      {
        title: 'Smart Table',
        link: '/pages/tables/smart-table',
      },
      {
        title: 'Tree Grid',
        link: '/pages/tables/tree-grid',
      },
    ],
  },
  {
    title: 'Miscellaneous',
    icon: 'shuffle-2-outline',
    children: [
      {
        title: '404',
        link: '/pages/miscellaneous/404',
      },
    ],
  },
  {
    title: 'Auth',
    icon: 'lock-outline',
    children: [
      {
        title: 'Login',
        link: '/auth/login',
      },
      {
        title: 'Register',
        link: '/auth/register',
      },
      {
        title: 'Request Password',
        link: '/auth/request-password',
      },
      {
        title: 'Reset Password',
        link: '/auth/reset-password',
      },
    ],
  },
];
