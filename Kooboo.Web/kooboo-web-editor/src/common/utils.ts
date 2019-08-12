import { getAllElement, isLink, isInEditorContainer } from "@/dom/utils";
import context from "./context";
import { SelectedDomEventArgs } from "@/events/SelectedDomEvent";
import { HOVER_BORDER_SKIP } from "./constants";
import "@webcomponents/shadydom";

export function delay(time: number) {
  return new Promise(rs => {
    setTimeout(rs, time);
  });
}

export function setElementClick() {
  for (const i of getAllElement(document.body, true)) {
    if (i instanceof HTMLElement) {
      if (isLink(i)) {
        let a = i.cloneNode(true);
        (a as any)._a = i;
        i.parentElement!.replaceChild(a, i);
      }
      holdUpClick(i);
    }
  }
}

export function holdUpClick(el: HTMLElement) {
  el.onclick = e => {
    if (e.isTrusted) {
      e.stopPropagation();
      e.preventDefault();
    }

    if (context.editing || isInEditorContainer(e)) return;
    let element = context.lastHoverDomEventArgs.closeElement;
    var args = new SelectedDomEventArgs(element);
    context.lastMouseEventArg = e;
    context.domChangeEvent.emit(args);
  };
}

export function createContainer() {
  let el = document.createElement("div");
  el.style.cssText = "all:unset !important";
  el.style.fontSize = "16px";
  document.documentElement.appendChild(el);
  let shadow = el.attachShadow({ mode: "open" });
  let root = document.createElement("div");
  root.id = HOVER_BORDER_SKIP;
  root.style.wordBreak = "break-all";
  root.style.fontFamily = `"Times New Roman",Times,serif`;
  root.style.fontStyle = "normal";
  root.style.fontVariant = "normal";
  shadow.appendChild(root);
  return root;
}
