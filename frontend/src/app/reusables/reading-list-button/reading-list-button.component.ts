import {Component, Input} from "@angular/core";
import {BookFeedItem} from "../../../models/stateModels";
import {ReadinglistService} from "../../../services/http/http.readinglist.service";

@Component({
  selector: 'reading-list-button',
  templateUrl: 'reading-list-button.component.html'
})
export class ReadingListButtonComponent {
  @Input() book: BookFeedItem = {};
  constructor(public readingListService: ReadinglistService) {
  }
}
