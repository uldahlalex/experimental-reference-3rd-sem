import {Component} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {GlobalState} from "../../../../services/state.global";
import {BookService} from "../../../../services/http/http.book.service";

@Component({
  selector: 'app-specific-book',
  templateUrl: 'specific-book.component.html'
})
export class SpecificBookComponent {

  constructor(private route: ActivatedRoute,
              public state: GlobalState,
              public bookService: BookService,
              public router: Router) {
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.bookService.getBookById(parseInt(<string>params.get('id')));
    });
  }

}
