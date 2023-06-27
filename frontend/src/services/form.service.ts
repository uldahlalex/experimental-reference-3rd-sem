import {Injectable} from "@angular/core";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Injectable({
  providedIn: 'root'
})
export class FormService {

  authFormData: AuthFormModel = {
    email: new FormControl(''),
    password: new FormControl(''),
  }

  searchBooksPreferencesFormData: SearchBooksPreferencesFormModel = {
    bookSearchTerm: new FormControl('', []),
    pageSize: new FormControl(5, [Validators.min(1)]),
    startAt: new FormControl(0, [Validators.min(0)]),
    orderBy: new FormControl('books.title', []),
  }

  searchAuthorsPreferencesFormData: SearchAuthorsPreferencesFormModel = {
    authorSearchTerm: new FormControl(''),
    pageSize: new FormControl(5),
    startAt: new FormControl(0),
    orderBy: new FormControl('name')
  }

  searchAuthorsPreferencesForm = new FormGroup<SearchAuthorsPreferencesFormModel>(this.searchAuthorsPreferencesFormData)
  searchBooksPreferencesForm = new FormGroup<SearchBooksPreferencesFormModel>(this.searchBooksPreferencesFormData)
  authForm = new FormGroup<AuthFormModel>(this.authFormData);


}

interface AuthFormModel {
  email: FormControl;
  password: FormControl;
}

interface SearchBooksPreferencesFormModel {
  bookSearchTerm: FormControl
  pageSize: FormControl,
  startAt: FormControl
  orderBy: FormControl
}

interface SearchAuthorsPreferencesFormModel {
  authorSearchTerm: FormControl
  pageSize: FormControl,
  startAt: FormControl
  orderBy: FormControl
}



