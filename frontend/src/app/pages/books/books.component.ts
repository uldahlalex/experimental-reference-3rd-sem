import {Component, OnInit} from '@angular/core';
import {GlobalState} from "../../../services/state.global";
import {ModalController} from "@ionic/angular";
import {SettingsModalComponent} from "./settings/settings-modal.component";
import {Router} from "@angular/router";
import {BookService} from "../../../services/http/http.book.service";
import {ReadinglistService} from "../../../services/http/http.readinglist.service";
import {FormService} from "../../../services/form.service";

@Component({
  selector: 'app-books',
  templateUrl: 'books.component.html'
})
export class BooksComponent implements OnInit {

  constructor(public state: GlobalState,
              public bookService: BookService,
              private modalCtrl: ModalController,
              public formService: FormService
  ) { }


  openModal() {
    this.modalCtrl.create({
      component: SettingsModalComponent
    }).then(res => {
      res.present();
    })
  }

  ngOnInit(): void {
    if(this.state.discoverFeed.recentlyAdded.length==0 && this.state.discoverFeed.notOnReadingList.length==0) {
      this.bookService.getDiscoverBooks();
    }
  }



}
