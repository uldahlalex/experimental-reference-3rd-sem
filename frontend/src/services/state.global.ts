import {Injectable} from '@angular/core';
import {AccountProfileData, BookFeedItem, DiscoverFeed} from "../models/stateModels";


@Injectable({
  providedIn: 'root'
})
export class GlobalState {


  books: BookFeedItem[] = [];
  accountProfileData: AccountProfileData = {};
  accountReadingList: BookFeedItem[] = [];
  currentBook: BookFeedItem = {};
  discoverFeed: DiscoverFeed = {
    recentlyAdded: [],
    notOnReadingList: []
  }


}
