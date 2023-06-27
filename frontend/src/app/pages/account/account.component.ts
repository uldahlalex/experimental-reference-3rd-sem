import {Component} from '@angular/core';
import {AnimationController, NavController} from "@ionic/angular";
import {GlobalState} from '../../../services/state.global';
import {AuthenticationService} from "../../../services/http/http.authentication.service";
import {HelperService} from "../../../services/helper.service";
import {FormService} from "../../../services/form.service";
import {Location} from "@angular/common";
import {BookService} from "../../../services/http/http.book.service";

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  animations: []
})
export class AccountComponent {


  constructor(
    public helper: HelperService,
    public state: GlobalState,
    public auth: AuthenticationService) {
  }



  switchAvatar() {
    this.auth.switchAvatar();
  }
}

