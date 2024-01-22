import { Component, OnInit } from '@angular/core'
import { ActivatedRoute } from '@angular/router'

@Component({
  selector: 'app-help',
  templateUrl: './help.component.html',
  styleUrls: ['./help.component.scss']
})
export class HelpComponent implements OnInit {


  mode: string | null = null;

  constructor(private activatedRouter: ActivatedRoute) {}
  ngOnInit(): void {
    this.mode = this.activatedRouter.snapshot.paramMap.get('mode');
    console.log(this.mode);
  }

}
