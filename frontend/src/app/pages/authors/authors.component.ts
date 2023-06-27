import { Component, OnInit } from '@angular/core';
import {GlobalState} from "../../../services/state.global";
import {FormService} from "../../../services/form.service";

@Component({
  selector: 'app-authors',
  templateUrl: './authors.component.html'
})
export class AuthorsComponent {

  constructor(public http: GlobalState, public formService: FormService) { }


}
