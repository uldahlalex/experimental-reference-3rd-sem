import {Component} from '@angular/core';
import {Location} from "@angular/common";
import {NavigationEnd, Router} from "@angular/router";
import {HelperService} from "../services/helper.service";


@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html'
})
export class AppComponent {

  constructor(public location: Location, public helper: HelperService) {

  }



}
