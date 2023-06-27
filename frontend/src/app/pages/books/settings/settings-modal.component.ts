import { Component, OnInit } from '@angular/core';
import {GlobalState} from "../../../../services/state.global";
import {ModalController} from "@ionic/angular";
import {BookService} from "../../../../services/http/http.book.service";
import {FormService} from "../../../../services/form.service";

@Component({
  selector: 'app-settings',
  templateUrl: './settings-modal.component.html'
})
export class SettingsModalComponent {

  constructor(public state: GlobalState,
              public formService: FormService,
              public modalCtrl: ModalController,
              public bookService: BookService) { }


}
