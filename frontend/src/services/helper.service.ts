import {Injectable} from "@angular/core";
import {GlobalState} from "./state.global";
import {ToastController} from "@ionic/angular";
import {AxiosError} from "axios";
import {BaseResponseDTO} from "../models/responseDTOs";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class HelperService {

  constructor(private state: GlobalState, private toastController: ToastController, private router: Router) {
  }


  logout() {
    this.emptyState();
    this.router.navigate(['account']).then(success => {
      this.toast('You have been logged out', "secondary");
    })

  }


  toast(message: string,
        color: string = "light",
        duration: number = 1250,
        position: "bottom" | "middle" | "top" | undefined = "bottom") {
    this.toastController.create({
      message: message,
      duration: duration,
      color: color,
      position: position
    }).then(toast => {
      toast.present().catch(e => {
        console.log('Could not present toast', e)
      })
    }).catch(e => {
      console.log('Could not create toast', e);
    })


  }

  emptyState() {
    localStorage.removeItem('jwt');
    this.state.accountProfileData = {};
    this.state.accountReadingList = [];
    this.state.books = [];
  }

  handleError(error: AxiosError<BaseResponseDTO>) {
    if (error.response!.status == 401) {
      this.emptyState();
      this.toast('Please log in again in order to continue', "secondary")
    } else {
      if(error.response!.data.messageToClient!=null && error.response!.data.messageToClient!='') {
        this.toast(error.response!.data.messageToClient, "danger", 3000);
      } else {
        this.toast(error.response!.data.toString(), "danger", 3000);
      }
    }
    console.error(error)

  }
}
