import {Injectable} from "@angular/core";
import axios, {AxiosError, AxiosRequestConfig} from 'axios';
import {AuthRequestDto} from "../../models/requestDTOs"
import jwtDecode from "jwt-decode";
import {AccountProfileData} from "../../models/stateModels";
import {GlobalState} from "../state.global";
import {HelperService} from "../helper.service";
import {AuthResponseDTO, BaseResponseDTO,} from "../../models/responseDTOs";
import {environment} from "../../environments/environment";
import {FormService} from "../form.service";
import {Router} from "@angular/router";
import {withJwt} from "./http.utils";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private helper: HelperService,
              private router: Router,
              private state: GlobalState,
              private formService: FormService) {
  }

  async register() {
    let formData: AuthRequestDto = this.formService.authForm.getRawValue();
    try {
      //SEND REQUEST
      const data = (await axios.post<AuthResponseDTO>(environment.baseUrl + '/register', formData)).data;
      //MUTATE STATE IF SUCCESS
      localStorage.setItem('jwt', data.responseData);
      this.state.accountProfileData = jwtDecode(data.responseData) as AccountProfileData;
      //GIVE USER RESPONSE
      this.helper.toast(data.messageToClient)
      //GIVE CALLER RESPONSE
    } catch (error) {

      if (error instanceof AxiosError) {
        this.helper.handleError(error);
      }
      //LET CALLER KNOW THERE WAS A FAILURE

    }

  }

  async logIn() {
    let formData: AuthRequestDto = this.formService.authForm.getRawValue();
    try {
      const data = (await axios.post<AuthResponseDTO>(environment.baseUrl + '/login', formData)).data;
      //MUTATE STATE IF SUCCESS
      localStorage.setItem('jwt', data.responseData);
      this.state.accountProfileData = jwtDecode(data.responseData) as AccountProfileData;
      //GIVE USER RESPONSE
      this.helper.toast(data.messageToClient)
      this.router.navigate(['books'])
      //GIVE CALLER RESPONSE
    } catch (error) {
      if (error instanceof AxiosError) {
        this.helper.handleError(error);
      }
      //LET CALLER KNOW THERE WAS A FAILURE

    }
  }

  async verifyTokenIntegrity() {
    try {
      const data = (await axios.get<AuthResponseDTO>(environment.baseUrl + '/verifyTokenIntegrity',
        withJwt()
      )).data;
      // const data = (await axios.get<AuthResponseDTO>(environment.baseUrl + '/verifyTokenIntegrity', {
      //   headers: {
      //     Authorization: localStorage.getItem('jwt')
      //   }
      // })).data;
      localStorage.setItem('jwt', data.responseData);
      this.state.accountProfileData = jwtDecode(data.responseData) as AccountProfileData;
      //this.helper.toast('Welcome back', 'primary', 1000, "top")
    } catch (error) {
      if (error instanceof AxiosError) {
        this.helper.handleError(error);
      }
      //LET CALLER KNOW THERE WAS A FAILURE

    }
  }

  async switchAvatar() {
    let pravatarId = Math.floor(Math.random() * 50) + 1;
    let config: AxiosRequestConfig = {
      headers: {
        Authorization: localStorage.getItem('jwt')
      }
    }
    try {
      const data = (await axios.put<BaseResponseDTO>(environment.baseUrl + '/avatar', {pravatarId: pravatarId}, config)).data;
      this.state.accountProfileData.pravatarId = pravatarId;
      this.helper.toast(data.messageToClient)
    } catch (error) {
      if (error instanceof AxiosError) {
        this.helper.handleError(error);
      }
      //LET CALLER KNOW THERE WAS A FAILURE


    }
  }
}
