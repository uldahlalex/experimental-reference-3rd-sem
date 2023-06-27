import {Injectable} from "@angular/core";
import {BaseResponseDTO, GetReadingListDTO} from "../../models/responseDTOs";
import {BookFeedItem} from "../../models/stateModels";
import {GlobalState} from "../state.global";
import axios, {AxiosError} from "axios";
import {HelperService} from "../helper.service";
import {environment} from "../../environments/environment";


@Injectable({
  providedIn: 'root'
})
export class ReadinglistService {

  constructor(private state: GlobalState,
              private helper: HelperService) {

  }

  async getReadingList() {
    try {
      const res = await axios
        .get<GetReadingListDTO>(environment.baseUrl + '/myReadingList',
          {
            headers:
              {
                Authorization: localStorage.getItem('jwt')
              }
          })
      this.state.accountReadingList = res.data.responseData;

    } catch (error) {
      if(error instanceof AxiosError) {
        this.helper.handleError(error);
      }

    }

  }

  async addToReadingList(book: BookFeedItem) {
    try {
      const data = (await axios.post<BaseResponseDTO>(environment.baseUrl + '/addToMyReadingList/book/' + book.bookId, {}, {headers: {Authorization: localStorage.getItem('jwt')}})).data;
      book.isOnMyReadingList = true;
      this.state.accountReadingList?.push(book)
      this.helper.toast(data.messageToClient, "success")
    } catch(error) {
      if(error instanceof AxiosError) {
        this.helper.handleError(error);
      }

    }
  }

  async removeFromReadingList(book: BookFeedItem) {
    try {
      const res = (await axios.delete<BaseResponseDTO>(environment.baseUrl + '/removeFromMyReadingList/book/' + book.bookId, {headers: {Authorization: localStorage.getItem('jwt')}})).data;
      book.isOnMyReadingList = false;
      this.state.accountReadingList = this.state.accountReadingList?.filter(b => b.bookId != book.bookId);
      this.helper.toast(res.messageToClient, "success");
    } catch (error) {
      if (error instanceof AxiosError) {
        this.helper.handleError(error)
      }

    }
  }

}
