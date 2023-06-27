import {Component, Input} from "@angular/core";
import {Router} from "@angular/router";
import {ReadinglistService} from "../../../services/http/http.readinglist.service";
import {BookFeedItem} from "../../../models/stateModels";

@Component({
  selector: 'app-book-card',
  templateUrl: 'book-card.component.html'
})
export class BookCardComponent {
  @Input() book: BookFeedItem = {};

  constructor(public router: Router, public readingListService: ReadinglistService) {
  }


  navigateToBook(id: number | undefined) {
    this.router.navigate(['books/'+id]);
  }
}
