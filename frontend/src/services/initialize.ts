import {AuthenticationService} from "./http/http.authentication.service";
import {BookService} from "./http/http.book.service";
import {ReadinglistService} from "./http/http.readinglist.service";
import jwtDecode from "jwt-decode";
import {AccountProfileData} from "../models/stateModels";

export function initialize(
  auth: AuthenticationService,
  bookService: BookService,
  readingListService: ReadinglistService,
): () => void {
  return async () => {
    let jwt = localStorage.getItem('jwt');
    if (jwt != undefined && (jwtDecode(jwt) as AccountProfileData).endUserId != undefined) {
      return auth.verifyTokenIntegrity().then(r => {
        bookService.getDiscoverBooks().then(r => {
          readingListService.getReadingList()
        })
      })
    }
  }
}
