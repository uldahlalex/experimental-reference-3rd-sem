import {Injectable} from "@angular/core";
import {DiscoverBooksDTO, GetBookById, GetBooksForFeedDTO} from "../../models/responseDTOs";
import {GlobalState} from "../state.global";
import {HelperService} from "../helper.service";
import axios, {AxiosError, AxiosRequestConfig} from "axios";
import {environment} from "../../environments/environment";
import {FormService} from "../form.service";
import {GetBooksParams} from "../../models/requestDTOs";

@Injectable({
  providedIn: 'root'
})
export class BookService {

  constructor(private state: GlobalState,
              private helper: HelperService,
              private formService: FormService) {

  }

  async getBooks() {
    if (this.formService.searchBooksPreferencesForm.invalid) {
      this.helper.toast("Form is not valid", "warning")
      return;
    } else {
      const searchParams: GetBooksParams = this.formService.searchBooksPreferencesForm.getRawValue();
      try {
        const res = await axios.get<GetBooksForFeedDTO>(environment.baseUrl + '/books', {
          params: searchParams, headers: {Authorization: localStorage.getItem('jwt')}
        })
        this.state.books = res.data.responseData;
      } catch (error) {
        if (error instanceof AxiosError) {
          this.helper.handleError(error);
        }

      }
    }


  }

  async getBookById(number: number) {
    try {
      const data = (await axios.get<GetBookById>(environment.baseUrl + '/books/' + number, {headers: {Authorization: localStorage.getItem('jwt')}})).data;
      this.state.currentBook = data.responseData;
    } catch (error) {
      if (error instanceof AxiosError) {
        this.helper.handleError(error);
      }

    }
  }

  async getDiscoverBooks() {
    const options: AxiosRequestConfig = {
      headers: {
        Authorization: localStorage.getItem('jwt')
      }
    }
    try {
      const data = (await axios.get<DiscoverBooksDTO>(environment.baseUrl + '/discover', options)).data;
      this.state.discoverFeed = data.responseData;
    } catch (error) {
      if (error instanceof AxiosError) {
        this.helper.handleError(error);
      }

    }
  }

  deleteBook(bookId: number | undefined, throwerr: boolean) {
    try {

    } catch (error) {
      if (error instanceof AxiosError) {
        this.helper.handleError(error);
      }
      if (throwerr) {
        throw error;
      }
    }
  }
}
