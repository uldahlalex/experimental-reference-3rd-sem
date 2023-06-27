import {Component, OnInit} from "@angular/core";
import {AuthenticationService} from "../../../services/http/http.authentication.service";
import {FormService} from "../../../services/form.service";
import {GlobalState} from "../../../services/state.global";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: 'login.component.html'
})
export class LoginComponent implements OnInit {

  constructor(public auth: AuthenticationService,
              private router: Router,
              public state: GlobalState,
              public formService: FormService) {
  }
  logIn() {
    this.auth.logIn().then(r => {

    })
  }

  ngOnInit(): void {
    this.auth.verifyTokenIntegrity().then(r => {
      console.log('t')
      this.router.navigate(['account'])
    })
  }
}
