import {Injectable} from "@angular/core";
import {GetBooksForFeedDTO} from "../../models/responseDTOs";
import axios, {AxiosError} from "axios";
import {ToastController} from "@ionic/angular";
import {GlobalState} from "../state.global";
import {HelperService} from "../helper.service";

@Injectable({
  providedIn: 'root'
})
export class AuthorService {


  constructor(private state: GlobalState, private helper: HelperService) {

  }



}
