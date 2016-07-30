using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;
using PhoneDumpClient.Services;
using Xamarin.Forms;
using System.Diagnostics;
using System.Windows.Input;
using PhoneDump.Contract.Services;
using PhoneDump.Entity.Dumps;
using XamlingCore.Portable.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using PhoneDump.Services.Messages;
using System.IO;

namespace PhoneDumpClient.View
{
    public class HomeViewModel : XViewModel
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenTestService _testService;
        private readonly IFilePickerService _filePickerService;
        private readonly ISendDumpService _sendDumpService;

        public string _mainText;

        public ICommand TestButtonCommand { get; set; }

        public ICommand TestTextButtonCommand { get; set; }
        public ICommand TestUrlButtonCommand { get; set; }
        public ICommand TestPdfButtonCommand { get; set; }
        public ICommand TestImageButtonCommand { get; set; }

        private ImageSource _dumpSource;
        public ImageSource DumpSource { get { return _dumpSource; }
            set { _dumpSource = value;  OnPropertyChanged();  } }

        public HomeViewModel(ITokenService tokenService, ITokenTestService testService,
            IFilePickerService filePickerService,
            ISendDumpService sendDumpService)
        {

            this.Register<NewDumpMessage>(_onNewDumpMessage);

            TestTextButtonCommand = new XCommand(_onTestTextButton);
            TestUrlButtonCommand = new XCommand(_onTestUrlButtonCommand);
            TestPdfButtonCommand = new XCommand(_onTestPdfButtonCommand);
            TestImageButtonCommand = new XCommand(_onTestImageButton);

            _tokenService = tokenService;
            _testService = testService;
            _filePickerService = filePickerService;
            _sendDumpService = sendDumpService;

            _timer();

        }

        private void _onTestImageButton()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var str = await _filePickerService.GetFileStringAsync();
                var entity = new DumpWireEntity
                {
                    Id = Guid.NewGuid(),
                    EncodedData = str,
                    MediaType = "image/jpeg",
                    RawData = "Image file"
                };
                await _sendDumpService.SendDump(entity);
            });
        }

        private void _onTestPdfButtonCommand()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var entity = new DumpWireEntity
                {
                    Id = Guid.NewGuid(),
                    EncodedData = string.Empty,
                    MediaType = "application/pdf",
                    RawData = "PDF not implemented yet"
                };
                await _sendDumpService.SendDump(entity);
            });
        }

        private void _onTestUrlButtonCommand()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var entity = new DumpWireEntity
                {
                    Id = Guid.NewGuid(),
                    EncodedData = string.Empty,
                    MediaType = "text/uri-list",
                    RawData = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
                };
                await _sendDumpService.SendDump(entity);
            });
        }

        private void _onTestTextButton()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var entity = new DumpWireEntity
                {
                    Id = Guid.NewGuid(),
                    EncodedData = string.Empty,
                    MediaType = "text/plain",
                    RawData = "Just a little bit of text"
                };
                await _sendDumpService.SendDump(entity);
            });
        }

        async void _timer()
        {
            while (true)
            {
                MainText = DateTime.Now.ToString();
                await Task.Delay(1000);
            }
        }

        void _onNewDumpMessage(object message)
        {
            var m = message as NewDumpMessage;

            if (m?.Entity == null)
            {
                return;
            }

            _processImage(m.Entity);
        }

        void _processImage(DumpWireEntity dump)
        {
            RawMessage = string.Empty;

            // Image types from http://www.iana.org/assignments/media-types/media-types.xhtml
            switch (dump.MediaType)
            {
                // Anything that might be able to be displayed in the browser window
                case "application/pdf":
                case "text/plain":
                case "text/uri-list":

                    DumpSource = null;

                    Uri target = null;
                    if (Uri.TryCreate(dump.RawData, UriKind.Absolute, out target))
                    {
                        TargetUrl = target.ToString();
                    }
                    else
                    {
                        TargetUrl = string.Empty;
                    }

                    RawMessage = dump.RawData;

                    break;

                case "image/bmp":
                case "image/png":
                case "image/jpg":
                case "image/jpeg":
                    TargetUrl = string.Empty;

                    // Overtake string-data from object in variable
                    //string cFotoBase64 = dump.EncodedData; // Overtake string-data from object in variable
                    //                                       // Convert in Byte-Array with encoding
                    Byte[] ImageFotoBase64 = System.Convert.FromBase64String(dump.EncodedData);
                    // Create Image and set stream from converted Byte-Array as source
                    DumpSource = ImageSource.FromStream(() => new MemoryStream(ImageFotoBase64));//, WidthRequest = 200, HeightRequest = 200, BackgroundColor = Color.Aqua, };
                    RawMessage = "Received Image File";
                    break;

                // video handler
                case "video/mp4":
                    TargetUrl = string.Empty;

                    // Find the locally written file and set the media element's source

                    break;

                #region Unused MIME Types
                #region application
                // application
                case "application/1d-interleaved-parityfec":
                case "application/3gpdash-qoe-report+xml":
                case "application/3gpp-ims+xml":
                case "application/A2L":
                case "application/activemessage":
                case "application/alto-costmap+json":
                case "application/alto-costmapfilter+json":
                case "application/alto-directory+json":
                case "application/alto-endpointprop+json":
                case "application/alto-endpointpropparams+json":
                case "application/alto-endpointcost+json":
                case "application/alto-endpointcostparams+json":
                case "application/alto-error+json":
                case "application/alto-networkmapfilter+json":
                case "application/alto-networkmap+json":
                case "application/AML":
                case "application/andrew-inset":
                case "application/applefile":
                case "application/ATF":
                case "application/ATFX":
                case "application/atom+xml":
                case "application/atomcat+xml":
                case "application/atomdeleted+xml":
                case "application/atomicmail":
                case "application/atomsvc+xml":
                case "application/ATXML":
                case "application/auth-policy+xml":
                case "application/bacnet-xdd+zip":
                case "application/batch-SMTP":
                case "application/beep+xml":
                case "application/calendar+json":
                case "application/calendar+xml":
                case "application/call-completion":
                case "application/CALS-1840":
                case "application/cbor":
                case "application/ccmp+xml":
                case "application/ccxml+xml":
                case "application/CDFX+XML":
                case "application/cdmi-capability":
                case "application/cdmi-container":
                case "application/cdmi-domain":
                case "application/cdmi-object":
                case "application/cdmi-queue":
                case "application/cdni":
                case "application/CEA":
                case "application/cea-2018+xml":
                case "application/cellml+xml":
                case "application/cfw":
                case "application/cms":
                case "application/cnrp+xml":
                case "application/coap-group+json":
                case "application/commonground":
                case "application/conference-info+xml":
                case "application/cpl+xml":
                case "application/csrattrs":
                case "application/csta+xml":
                case "application/CSTAdata+xml":
                case "application/csvm+json":
                case "application/cybercash":
                case "application/dash+xml":
                case "application/dashdelta":
                case "application/davmount+xml":
                case "application/dca-rft":
                case "application/DCD":
                case "application/dec-dx":
                case "application/dialog-info+xml":
                case "application/dicom":
                case "application/DII":
                case "application/DIT":
                case "application/dns":
                case "application/dskpp+xml":
                case "application/dssc+der":
                case "application/dssc+xml":
                case "application/dvcs":
                case "application/ecmascript":
                case "application/EDI-consent":
                case "application/EDIFACT":
                case "application/EDI-X12":
                case "application/efi":
                case "application/EmergencyCallData.Comment+xml":
                case "application/EmergencyCallData.DeviceInfo+xml":
                case "application/EmergencyCallData.ProviderInfo+xml":
                case "application/EmergencyCallData.ServiceInfo+xml":
                case "application/EmergencyCallData.SubscriberInfo+xml":
                case "application/emotionml+xml":
                case "application/encaprtp":
                case "application/epp+xml":
                case "application/epub+zip":
                case "application/eshop":
                case "application/example":
                case "application/fastinfoset":
                case "application/fastsoap":
                case "application/fdt+xml":
                case "application/fits":
                case "application/font-sfnt":
                case "application/font-tdpfr":
                case "application/font-woff":
                case "application/framework-attributes+xml":
                case "application/geo+json":
                case "application/gzip":
                case "application/H224":
                case "application/held+xml":
                case "application/http":
                case "application/hyperstudio":
                case "application/ibe-key-request+xml":
                case "application/ibe-pkg-reply+xml":
                case "application/ibe-pp-data":
                case "application/iges":
                case "application/im-iscomposing+xml":
                case "application/index":
                case "application/index.cmd":
                case "application/index-obj":
                case "application/index.response":
                case "application/index.vnd":
                case "application/inkml+xml":
                case "application/IOTP":
                case "application/ipfix":
                case "application/ipp":
                case "application/ISUP":
                case "application/its+xml":
                case "application/javascript":
                case "application/jose":
                case "application/jose+json":
                case "application/jrd+json":
                case "application/json":
                case "application/json-patch+json":
                case "application/json-seq":
                case "application/jwk+json":
                case "application/jwk-set+json":
                case "application/jwt":
                case "application/kpml-request+xml":
                case "application/kpml-response+xml":
                case "application/ld+json":
                case "application/lgr+xml":
                case "application/link-format":
                case "application/load-control+xml":
                case "application/lost+xml":
                case "application/lostsync+xml":
                case "application/LXF":
                case "application/mac-binhex40":
                case "application/macwriteii":
                case "application/mads+xml":
                case "application/marc":
                case "application/marcxml+xml":
                case "application/mathematica":
                case "application/mbms-associated-procedure-description+xml":
                case "application/mbms-deregister+xml":
                case "application/mbms-envelope+xml":
                case "application/mbms-msk-response+xml":
                case "application/mbms-msk+xml":
                case "application/mbms-protection-description+xml":
                case "application/mbms-reception-report+xml":
                case "application/mbms-register-response+xml":
                case "application/mbms-register+xml":
                case "application/mbms-schedule+xml":
                case "application/mbms-user-service-description+xml":
                case "application/mbox":
                case "application/media_control+xml":
                case "application/media-policy-dataset+xml":
                case "application/mediaservercontrol+xml":
                case "application/merge-patch+json":
                case "application/metalink4+xml":
                case "application/mets+xml":
                case "application/MF4":
                case "application/mikey":
                case "application/mods+xml":
                case "application/moss-keys":
                case "application/moss-signature":
                case "application/mosskey-data":
                case "application/mosskey-request":
                case "application/mp21":
                case "application/mp4":
                case "application/mpeg4-generic":
                case "application/mpeg4-iod":
                case "application/mpeg4-iod-xmt":
                case "application/mrb-consumer+xml":
                case "application/mrb-publish+xml":
                case "application/msc-ivr+xml":
                case "application/msc-mixer+xml":
                case "application/msword":
                case "application/mxf":
                case "application/nasdata":
                case "application/news-checkgroups":
                case "application/news-groupinfo":
                case "application/news-transmission":
                case "application/nlsml+xml":
                case "application/nss":
                case "application/ocsp-request":
                case "application/ocsp-response":
                case "application/octet-stream":
                case "application/ODA":
                case "application/ODX":
                case "application/oebps-package+xml":
                case "application/ogg":
                case "application/oxps":
                case "application/p2p-overlay+xml":
                case "application/patch-ops-error+xml":
                case "application/PDX":
                case "application/pgp-encrypted":
                case "application/pgp-signature":
                case "application/pidf-diff+xml":
                case "application/pidf+xml":
                case "application/pkcs10":
                case "application/pkcs7-mime":
                case "application/pkcs7-signature":
                case "application/pkcs8":
                case "application/pkcs12":
                case "application/pkix-attr-cert":
                case "application/pkix-cert":
                case "application/pkix-crl":
                case "application/pkix-pkipath":
                case "application/pkixcmp":
                case "application/pls+xml":
                case "application/poc-settings+xml":
                case "application/postscript":
                case "application/ppsp-tracker+json":
                case "application/problem+json":
                case "application/problem+xml":
                case "application/provenance+xml":
                case "application/prs.alvestrand.titrax-sheet":
                case "application/prs.cww":
                case "application/prs.hpub+zip":
                case "application/prs.nprend":
                case "application/prs.plucker":
                case "application/prs.rdf-xml-crypt":
                case "application/prs.xsf+xml":
                case "application/pskc+xml":
                case "application/rdf+xml":
                case "application/QSIG":
                case "application/raptorfec":
                case "application/rdap+json":
                case "application/reginfo+xml":
                case "application/relax-ng-compact-syntax":
                case "application/remote-printing":
                case "application/reputon+json":
                case "application/resource-lists-diff+xml":
                case "application/resource-lists+xml":
                case "application/rfc+xml":
                case "application/riscos":
                case "application/rlmi+xml":
                case "application/rls-services+xml":
                case "application/rpki-ghostbusters":
                case "application/rpki-manifest":
                case "application/rpki-roa":
                case "application/rpki-updown":
                case "application/rtf":
                case "application/rtploopback":
                case "application/rtx":
                case "application/samlassertion+xml":
                case "application/samlmetadata+xml":
                case "application/sbml+xml":
                case "application/scaip+xml":
                case "application/scim+json":
                case "application/scvp-cv-request":
                case "application/scvp-cv-response":
                case "application/scvp-vp-request":
                case "application/scvp-vp-response":
                case "application/sdp":
                case "application/sep-exi":
                case "application/sep+xml":
                case "application/session-info":
                case "application/set-payment":
                case "application/set-payment-initiation":
                case "application/set-registration":
                case "application/set-registration-initiation":
                case "application/SGML":
                case "application/sgml-open-catalog":
                case "application/shf+xml":
                case "application/sieve":
                case "application/simple-filter+xml":
                case "application/simple-message-summary":
                case "application/simpleSymbolContainer":
                case "application/slate":
                case "application/smil":
                case "application/smil+xml":
                case "application/smpte336m":
                case "application/soap+fastinfoset":
                case "application/soap+xml":
                case "application/spirits-event+xml":
                case "application/sql":
                case "application/srgs":
                case "application/srgs+xml":
                case "application/sru+xml":
                case "application/ssml+xml":
                case "application/tamp-apex-update":
                case "application/tamp-apex-update-confirm":
                case "application/tamp-community-update":
                case "application/tamp-community-update-confirm":
                case "application/tamp-error":
                case "application/tamp-sequence-adjust":
                case "application/tamp-sequence-adjust-confirm":
                case "application/tamp-status-query":
                case "application/tamp-status-response":
                case "application/tamp-update":
                case "application/tamp-update-confirm":
                case "application/tei+xml":
                case "application/thraud+xml":
                case "application/timestamp-query":
                case "application/timestamp-reply":
                case "application/timestamped-data":
                case "application/ttml+xml":
                case "application/tve-trigger":
                case "application/ulpfec":
                case "application/urc-grpsheet+xml":
                case "application/urc-ressheet+xml":
                case "application/urc-targetdesc+xml":
                case "application/urc-uisocketdesc+xml":
                case "application/vcard+json":
                case "application/vcard+xml":
                case "application/vemmi":
                case "application/vnd.3gpp.access-transfer-events+xml":
                case "application/vnd.3gpp.bsf+xml":
                case "application/vnd.3gpp.mid-call+xml":
                case "application/vnd.3gpp.pic-bw-large":
                case "application/vnd.3gpp.pic-bw-small":
                case "application/vnd.3gpp.pic-bw-var":
                case "application/vnd.3gpp-prose-pc3ch+xml":
                case "application/vnd.3gpp-prose+xml":
                case "application/vnd.3gpp.sms":
                case "application/vnd.3gpp.sms+xml":
                case "application/vnd.3gpp.srvcc-ext+xml":
                case "application/vnd.3gpp.SRVCC-info+xml":
                case "application/vnd.3gpp.state-and-event-info+xml":
                case "application/vnd.3gpp.ussd+xml":
                case "application/vnd.3gpp2.bcmcsinfo+xml":
                case "application/vnd.3gpp2.sms":
                case "application/vnd.3gpp2.tcap":
                case "application/vnd.3lightssoftware.imagescal":
                case "application/vnd.3M.Post-it-Notes":
                case "application/vnd.accpac.simply.aso":
                case "application/vnd.accpac.simply.imp":
                case "application/vnd-acucobol":
                case "application/vnd.acucorp":
                case "application/vnd.adobe.flash-movie":
                case "application/vnd.adobe.formscentral.fcdt":
                case "application/vnd.adobe.fxp":
                case "application/vnd.adobe.partial-upload":
                case "application/vnd.adobe.xdp+xml":
                case "application/vnd.adobe.xfdf":
                case "application/vnd.aether.imp":
                case "application/vnd.ah-barcode":
                case "application/vnd.ahead.space":
                case "application/vnd.airzip.filesecure.azf":
                case "application/vnd.airzip.filesecure.azs":
                case "application/vnd.amazon.mobi8-ebook":
                case "application/vnd.americandynamics.acc":
                case "application/vnd.amiga.ami":
                case "application/vnd.amundsen.maze+xml":
                case "application/vnd.anki":
                case "application/vnd.anser-web-certificate-issue-initiation":
                case "application/vnd.antix.game-component":
                case "application/vnd.apache.thrift.binary":
                case "application/vnd.apache.thrift.compact":
                case "application/vnd.apache.thrift.json":
                case "application/vnd.api+json":
                case "application/vnd.apple.mpegurl":
                case "application/vnd.apple.installer+xml":
                case "application/vnd.arastra.swi":
                case "application/vnd.aristanetworks.swi":
                case "application/vnd.artsquare":
                case "application/vnd.astraea-software.iota":
                case "application/vnd.audiograph":
                case "application/vnd.autopackage":
                case "application/vnd.avistar+xml":
                case "application/vnd.balsamiq.bmml+xml":
                case "application/vnd.balsamiq.bmpr":
                case "application/vnd.bekitzur-stech+json":
                case "application/vnd.biopax.rdf+xml":
                case "application/vnd.blueice.multipass":
                case "application/vnd.bluetooth.ep.oob":
                case "application/vnd.bluetooth.le.oob":
                case "application/vnd.bmi":
                case "application/vnd.businessobjects":
                case "application/vnd.cab-jscript":
                case "application/vnd.canon-cpdl":
                case "application/vnd.canon-lips":
                case "application/vnd.cendio.thinlinc.clientconf":
                case "application/vnd.century-systems.tcp_stream":
                case "application/vnd.chemdraw+xml":
                case "application/vnd.chess-pgn":
                case "application/vnd.chipnuts.karaoke-mmd":
                case "application/vnd.cinderella":
                case "application/vnd.cirpack.isdn-ext":
                case "application/vnd.citationstyles.style+xml":
                case "application/vnd.claymore":
                case "application/vnd.cloanto.rp9":
                case "application/vnd.clonk.c4group":
                case "application/vnd.cluetrust.cartomobile-config":
                case "application/vnd.cluetrust.cartomobile-config-pkg":
                case "application/vnd.coffeescript":
                case "application/vnd.collection.doc+json":
                case "application/vnd.collection+json":
                case "application/vnd.collection.next+json":
                case "application/vnd.comicbook+zip":
                case "application/vnd.commerce-battelle":
                case "application/vnd.commonspace":
                case "application/vnd.coreos.ignition+json":
                case "application/vnd.cosmocaller":
                case "application/vnd.contact.cmsg":
                case "application/vnd.crick.clicker":
                case "application/vnd.crick.clicker.keyboard":
                case "application/vnd.crick.clicker.palette":
                case "application/vnd.crick.clicker.template":
                case "application/vnd.crick.clicker.wordbank":
                case "application/vnd.criticaltools.wbs+xml":
                case "application/vnd.ctc-posml":
                case "application/vnd.ctct.ws+xml":
                case "application/vnd.cups-pdf":
                case "application/vnd.cups-postscript":
                case "application/vnd.cups-ppd":
                case "application/vnd.cups-raster":
                case "application/vnd.cups-raw":
                case "application/vnd-curl":
                case "application/vnd.cyan.dean.root+xml":
                case "application/vnd.cybank":
                case "application/vnd-dart":
                case "application/vnd.data-vision.rdz":
                case "application/vnd.debian.binary-package":
                case "application/vnd.dece.data":
                case "application/vnd.dece.ttml+xml":
                case "application/vnd.dece.unspecified":
                case "application/vnd.dece-zip":
                case "application/vnd.denovo.fcselayout-link":
                case "application/vnd.desmume-movie":
                case "application/vnd.dir-bi.plate-dl-nosuffix":
                case "application/vnd.dm.delegation+xml":
                case "application/vnd.dna":
                case "application/vnd.document+json":
                case "application/vnd.dolby.mobile.1":
                case "application/vnd.dolby.mobile.2":
                case "application/vnd.doremir.scorecloud-binary-document":
                case "application/vnd.dpgraph":
                case "application/vnd.dreamfactory":
                case "application/vnd.drive+json":
                case "application/vnd.dtg.local":
                case "application/vnd.dtg.local.flash":
                case "application/vnd.dtg.local-html":
                case "application/vnd.dvb.ait":
                case "application/vnd.dvb.dvbj":
                case "application/vnd.dvb.esgcontainer":
                case "application/vnd.dvb.ipdcdftnotifaccess":
                case "application/vnd.dvb.ipdcesgaccess":
                case "application/vnd.dvb.ipdcesgaccess2":
                case "application/vnd.dvb.ipdcesgpdd":
                case "application/vnd.dvb.ipdcroaming":
                case "application/vnd.dvb.iptv.alfec-base":
                case "application/vnd.dvb.iptv.alfec-enhancement":
                case "application/vnd.dvb.notif-aggregate-root+xml":
                case "application/vnd.dvb.notif-container+xml":
                case "application/vnd.dvb.notif-generic+xml":
                case "application/vnd.dvb.notif-ia-msglist+xml":
                case "application/vnd.dvb.notif-ia-registration-request+xml":
                case "application/vnd.dvb.notif-ia-registration-response+xml":
                case "application/vnd.dvb.notif-init+xml":
                case "application/vnd.dvb.pfr":
                case "application/vnd.dvb_service":
                case "application/vnd-dxr":
                case "application/vnd.dynageo":
                case "application/vnd.dzr":
                case "application/vnd.easykaraoke.cdgdownload":
                case "application/vnd.ecdis-update":
                case "application/vnd.ecowin.chart":
                case "application/vnd.ecowin.filerequest":
                case "application/vnd.ecowin.fileupdate":
                case "application/vnd.ecowin.series":
                case "application/vnd.ecowin.seriesrequest":
                case "application/vnd.ecowin.seriesupdate":
                case "application/vnd.emclient.accessrequest+xml":
                case "application/vnd.enliven":
                case "application/vnd.enphase.envoy":
                case "application/vnd.eprints.data+xml":
                case "application/vnd.epson.esf":
                case "application/vnd.epson.msf":
                case "application/vnd.epson.quickanime":
                case "application/vnd.epson.salt":
                case "application/vnd.epson.ssf":
                case "application/vnd.ericsson.quickcall":
                case "application/vnd.eszigno3+xml":
                case "application/vnd.etsi.aoc+xml":
                case "application/vnd.etsi.asic-s+zip":
                case "application/vnd.etsi.asic-e+zip":
                case "application/vnd.etsi.cug+xml":
                case "application/vnd.etsi.iptvcommand+xml":
                case "application/vnd.etsi.iptvdiscovery+xml":
                case "application/vnd.etsi.iptvprofile+xml":
                case "application/vnd.etsi.iptvsad-bc+xml":
                case "application/vnd.etsi.iptvsad-cod+xml":
                case "application/vnd.etsi.iptvsad-npvr+xml":
                case "application/vnd.etsi.iptvservice+xml":
                case "application/vnd.etsi.iptvsync+xml":
                case "application/vnd.etsi.iptvueprofile+xml":
                case "application/vnd.etsi.mcid+xml":
                case "application/vnd.etsi.mheg5":
                case "application/vnd.etsi.overload-control-policy-dataset+xml":
                case "application/vnd.etsi.pstn+xml":
                case "application/vnd.etsi.sci+xml":
                case "application/vnd.etsi.simservs+xml":
                case "application/vnd.etsi.timestamp-token":
                case "application/vnd.etsi.tsl+xml":
                case "application/vnd.etsi.tsl.der":
                case "application/vnd.eudora.data":
                case "application/vnd.ezpix-album":
                case "application/vnd.ezpix-package":
                case "application/vnd.f-secure.mobile":
                case "application/vnd.fastcopy-disk-image":
                case "application/vnd-fdf":
                case "application/vnd.fdsn.mseed":
                case "application/vnd.fdsn.seed":
                case "application/vnd.ffsns":
                case "application/vnd.filmit.zfc":
                case "application/vnd.fints":
                case "application/vnd.firemonkeys.cloudcell":
                case "application/vnd.FloGraphIt":
                case "application/vnd.fluxtime.clip":
                case "application/vnd.font-fontforge-sfd":
                case "application/vnd.framemaker":
                case "application/vnd.frogans.fnc":
                case "application/vnd.frogans.ltf":
                case "application/vnd.fsc.weblaunch":
                case "application/vnd.fujitsu.oasys":
                case "application/vnd.fujitsu.oasys2":
                case "application/vnd.fujitsu.oasys3":
                case "application/vnd.fujitsu.oasysgp":
                case "application/vnd.fujitsu.oasysprs":
                case "application/vnd.fujixerox.ART4":
                case "application/vnd.fujixerox.ART-EX":
                case "application/vnd.fujixerox.ddd":
                case "application/vnd.fujixerox.docuworks":
                case "application/vnd.fujixerox.docuworks.binder":
                case "application/vnd.fujixerox.docuworks.container":
                case "application/vnd.fujixerox.HBPL":
                case "application/vnd.fut-misnet":
                case "application/vnd.fuzzysheet":
                case "application/vnd.genomatix.tuxedo":
                case "application/vnd.geo+json":
                case "application/vnd.geocube+xml":
                case "application/vnd.geogebra.file":
                case "application/vnd.geogebra.tool":
                case "application/vnd.geometry-explorer":
                case "application/vnd.geonext":
                case "application/vnd.geoplan":
                case "application/vnd.geospace":
                case "application/vnd.gerber":
                case "application/vnd.globalplatform.card-content-mgt":
                case "application/vnd.globalplatform.card-content-mgt-response":
                case "application/vnd.gmx":
                case "application/vnd.google-earth.kml+xml":
                case "application/vnd.google-earth.kmz":
                case "application/vnd.gov.sk.e-form+xml":
                case "application/vnd.gov.sk.e-form+zip":
                case "application/vnd.gov.sk.xmldatacontainer+xml":
                case "application/vnd.grafeq":
                case "application/vnd.gridmp":
                case "application/vnd.groove-account":
                case "application/vnd.groove-help":
                case "application/vnd.groove-identity-message":
                case "application/vnd.groove-injector":
                case "application/vnd.groove-tool-message":
                case "application/vnd.groove-tool-template":
                case "application/vnd.groove-vcard":
                case "application/vnd.hal+json":
                case "application/vnd.hal+xml":
                case "application/vnd.HandHeld-Entertainment+xml":
                case "application/vnd.hbci":
                case "application/vnd.hcl-bireports":
                case "application/vnd.hdt":
                case "application/vnd.heroku+json":
                case "application/vnd.hhe.lesson-player":
                case "application/vnd.hp-HPGL":
                case "application/vnd.hp-hpid":
                case "application/vnd.hp-hps":
                case "application/vnd.hp-jlyt":
                case "application/vnd.hp-PCL":
                case "application/vnd.hp-PCLXL":
                case "application/vnd.httphone":
                case "application/vnd.hydrostatix.sof-data":
                case "application/vnd.hyperdrive+json":
                case "application/vnd.hzn-3d-crossword":
                case "application/vnd.ibm.afplinedata":
                case "application/vnd.ibm.electronic-media":
                case "application/vnd.ibm.MiniPay":
                case "application/vnd.ibm.modcap":
                case "application/vnd.ibm.rights-management":
                case "application/vnd.ibm.secure-container":
                case "application/vnd.iccprofile":
                case "application/vnd.ieee.1905":
                case "application/vnd.igloader":
                case "application/vnd.immervision-ivp":
                case "application/vnd.immervision-ivu":
                case "application/vnd.ims.imsccv1p1":
                case "application/vnd.ims.imsccv1p2":
                case "application/vnd.ims.imsccv1p3":
                case "application/vnd.ims.lis.v2.result+json":
                case "application/vnd.ims.lti.v2.toolconsumerprofile+json":
                case "application/vnd.ims.lti.v2.toolproxy.id+json":
                case "application/vnd.ims.lti.v2.toolproxy+json":
                case "application/vnd.ims.lti.v2.toolsettings+json":
                case "application/vnd.ims.lti.v2.toolsettings.simple+json":
                case "application/vnd.informedcontrol.rms+xml":
                case "application/vnd.infotech.project":
                case "application/vnd.infotech.project+xml":
                case "application/vnd.informix-visionary":
                case "application/vnd.innopath.wamp.notification":
                case "application/vnd.insors.igm":
                case "application/vnd.intercon.formnet":
                case "application/vnd.intergeo":
                case "application/vnd.intertrust.digibox":
                case "application/vnd.intertrust.nncp":
                case "application/vnd.intu.qbo":
                case "application/vnd.intu.qfx":
                case "application/vnd.iptc.g2.catalogitem+xml":
                case "application/vnd.iptc.g2.conceptitem+xml":
                case "application/vnd.iptc.g2.knowledgeitem+xml":
                case "application/vnd.iptc.g2.newsitem+xml":
                case "application/vnd.iptc.g2.newsmessage+xml":
                case "application/vnd.iptc.g2.packageitem+xml":
                case "application/vnd.iptc.g2.planningitem+xml":
                case "application/vnd.ipunplugged.rcprofile":
                case "application/vnd.irepository.package+xml":
                case "application/vnd.is-xpr":
                case "application/vnd.isac.fcs":
                case "application/vnd.jam":
                case "application/vnd.japannet-directory-service":
                case "application/vnd.japannet-jpnstore-wakeup":
                case "application/vnd.japannet-payment-wakeup":
                case "application/vnd.japannet-registration":
                case "application/vnd.japannet-registration-wakeup":
                case "application/vnd.japannet-setstore-wakeup":
                case "application/vnd.japannet-verification":
                case "application/vnd.japannet-verification-wakeup":
                case "application/vnd.jcp.javame.midlet-rms":
                case "application/vnd.jisp":
                case "application/vnd.joost.joda-archive":
                case "application/vnd.jsk.isdn-ngn":
                case "application/vnd.kahootz":
                case "application/vnd.kde.karbon":
                case "application/vnd.kde.kchart":
                case "application/vnd.kde.kformula":
                case "application/vnd.kde.kivio":
                case "application/vnd.kde.kontour":
                case "application/vnd.kde.kpresenter":
                case "application/vnd.kde.kspread":
                case "application/vnd.kde.kword":
                case "application/vnd.kenameaapp":
                case "application/vnd.kidspiration":
                case "application/vnd.Kinar":
                case "application/vnd.koan":
                case "application/vnd.kodak-descriptor":
                case "application/vnd.las.las+xml":
                case "application/vnd.liberty-request+xml":
                case "application/vnd.llamagraphics.life-balance.desktop":
                case "application/vnd.llamagraphics.life-balance.exchange+xml":
                case "application/vnd.lotus-1-2-3":
                case "application/vnd.lotus-approach":
                case "application/vnd.lotus-freelance":
                case "application/vnd.lotus-notes":
                case "application/vnd.lotus-organizer":
                case "application/vnd.lotus-screencam":
                case "application/vnd.lotus-wordpro":
                case "application/vnd.macports.portpkg":
                case "application/vnd.mapbox-vector-tile":
                case "application/vnd.marlin.drm.actiontoken+xml":
                case "application/vnd.marlin.drm.conftoken+xml":
                case "application/vnd.marlin.drm.license+xml":
                case "application/vnd.marlin.drm.mdcf":
                case "application/vnd.mason+json":
                case "application/vnd.maxmind.maxmind-db":
                case "application/vnd.mcd":
                case "application/vnd.medcalcdata":
                case "application/vnd.mediastation.cdkey":
                case "application/vnd.meridian-slingshot":
                case "application/vnd.MFER":
                case "application/vnd.mfmp":
                case "application/vnd.micro+json":
                case "application/vnd.micrografx.flo":
                case "application/vnd.micrografx-igx":
                case "application/vnd.microsoft.portable-executable":
                case "application/vnd.miele+json":
                case "application/vnd-mif":
                case "application/vnd.minisoft-hp3000-save":
                case "application/vnd.mitsubishi.misty-guard.trustweb":
                case "application/vnd.Mobius.DAF":
                case "application/vnd.Mobius.DIS":
                case "application/vnd.Mobius.MBK":
                case "application/vnd.Mobius.MQY":
                case "application/vnd.Mobius.MSL":
                case "application/vnd.Mobius.PLC":
                case "application/vnd.Mobius.TXF":
                case "application/vnd.mophun.application":
                case "application/vnd.mophun.certificate":
                case "application/vnd.motorola.flexsuite":
                case "application/vnd.motorola.flexsuite.adsi":
                case "application/vnd.motorola.flexsuite.fis":
                case "application/vnd.motorola.flexsuite.gotap":
                case "application/vnd.motorola.flexsuite.kmr":
                case "application/vnd.motorola.flexsuite.ttc":
                case "application/vnd.motorola.flexsuite.wem":
                case "application/vnd.motorola.iprm":
                case "application/vnd.mozilla.xul+xml":
                case "application/vnd.ms-artgalry":
                case "application/vnd.ms-asf":
                case "application/vnd.ms-cab-compressed":
                case "application/vnd.ms-3mfdocument":
                case "application/vnd.ms-excel":
                case "application/vnd.ms-excel.addin.macroEnabled.12":
                case "application/vnd.ms-excel.sheet.binary.macroEnabled.12":
                case "application/vnd.ms-excel.sheet.macroEnabled.12":
                case "application/vnd.ms-excel.template.macroEnabled.12":
                case "application/vnd.ms-fontobject":
                case "application/vnd.ms-htmlhelp":
                case "application/vnd.ms-ims":
                case "application/vnd.ms-lrm":
                case "application/vnd.ms-office.activeX+xml":
                case "application/vnd.ms-officetheme":
                case "application/vnd.ms-playready.initiator+xml":
                case "application/vnd.ms-powerpoint":
                case "application/vnd.ms-powerpoint.addin.macroEnabled.12":
                case "application/vnd.ms-powerpoint.presentation.macroEnabled.12":
                case "application/vnd.ms-powerpoint.slide.macroEnabled.12":
                case "application/vnd.ms-powerpoint.slideshow.macroEnabled.12":
                case "application/vnd.ms-powerpoint.template.macroEnabled.12":
                case "application/vnd.ms-PrintDeviceCapabilities+xml":
                case "application/vnd.ms-PrintSchemaTicket+xml":
                case "application/vnd.ms-project":
                case "application/vnd.ms-tnef":
                case "application/vnd.ms-windows.devicepairing":
                case "application/vnd.ms-windows.nwprinting.oob":
                case "application/vnd.ms-windows.printerpairing":
                case "application/vnd.ms-windows.wsd.oob":
                case "application/vnd.ms-wmdrm.lic-chlg-req":
                case "application/vnd.ms-wmdrm.lic-resp":
                case "application/vnd.ms-wmdrm.meter-chlg-req":
                case "application/vnd.ms-wmdrm.meter-resp":
                case "application/vnd.ms-word.document.macroEnabled.12":
                case "application/vnd.ms-word.template.macroEnabled.12":
                case "application/vnd.ms-works":
                case "application/vnd.ms-wpl":
                case "application/vnd.ms-xpsdocument":
                case "application/vnd.msa-disk-image":
                case "application/vnd.mseq":
                case "application/vnd.msign":
                case "application/vnd.multiad.creator":
                case "application/vnd.multiad.creator.cif":
                case "application/vnd.musician":
                case "application/vnd.music-niff":
                case "application/vnd.muvee.style":
                case "application/vnd.mynfc":
                case "application/vnd.ncd.control":
                case "application/vnd.ncd.reference":
                case "application/vnd.nervana":
                case "application/vnd.netfpx":
                case "application/vnd.neurolanguage.nlu":
                case "application/vnd.nintendo.snes.rom":
                case "application/vnd.nintendo.nitro.rom":
                case "application/vnd.nitf":
                case "application/vnd.noblenet-directory":
                case "application/vnd.noblenet-sealer":
                case "application/vnd.noblenet-web":
                case "application/vnd.nokia.catalogs":
                case "application/vnd.nokia.conml+wbxml":
                case "application/vnd.nokia.conml+xml":
                case "application/vnd.nokia.iptv.config+xml":
                case "application/vnd.nokia.iSDS-radio-presets":
                case "application/vnd.nokia.landmark+wbxml":
                case "application/vnd.nokia.landmark+xml":
                case "application/vnd.nokia.landmarkcollection+xml":
                case "application/vnd.nokia.ncd":
                case "application/vnd.nokia.n-gage.ac+xml":
                case "application/vnd.nokia.n-gage.data":
                case "application/vnd.nokia.n-gage.symbian.install":
                case "application/vnd.nokia.pcd+wbxml":
                case "application/vnd.nokia.pcd+xml":
                case "application/vnd.nokia.radio-preset":
                case "application/vnd.nokia.radio-presets":
                case "application/vnd.novadigm.EDM":
                case "application/vnd.novadigm.EDX":
                case "application/vnd.novadigm.EXT":
                case "application/vnd.ntt-local.content-share":
                case "application/vnd.ntt-local.file-transfer":
                case "application/vnd.ntt-local.ogw_remote-access":
                case "application/vnd.ntt-local.sip-ta_remote":
                case "application/vnd.ntt-local.sip-ta_tcp_stream":
                case "application/vnd.oasis.opendocument.chart":
                case "application/vnd.oasis.opendocument.chart-template":
                case "application/vnd.oasis.opendocument.database":
                case "application/vnd.oasis.opendocument.formula":
                case "application/vnd.oasis.opendocument.formula-template":
                case "application/vnd.oasis.opendocument.graphics":
                case "application/vnd.oasis.opendocument.graphics-template":
                case "application/vnd.oasis.opendocument.image":
                case "application/vnd.oasis.opendocument.image-template":
                case "application/vnd.oasis.opendocument.presentation":
                case "application/vnd.oasis.opendocument.presentation-template":
                case "application/vnd.oasis.opendocument.spreadsheet":
                case "application/vnd.oasis.opendocument.spreadsheet-template":
                case "application/vnd.oasis.opendocument.text":
                case "application/vnd.oasis.opendocument.text-master":
                case "application/vnd.oasis.opendocument.text-template":
                case "application/vnd.oasis.opendocument.text-web":
                case "application/vnd.obn":
                case "application/vnd.oftn.l10n+json":
                case "application/vnd.oipf.contentaccessdownload+xml":
                case "application/vnd.oipf.contentaccessstreaming+xml":
                case "application/vnd.oipf.cspg-hexbinary":
                case "application/vnd.oipf.dae.svg+xml":
                case "application/vnd.oipf.dae.xhtml+xml":
                case "application/vnd.oipf.mippvcontrolmessage+xml":
                case "application/vnd.oipf.pae.gem":
                case "application/vnd.oipf.spdiscovery+xml":
                case "application/vnd.oipf.spdlist+xml":
                case "application/vnd.oipf.ueprofile+xml":
                case "application/vnd.oipf.userprofile+xml":
                case "application/vnd.olpc-sugar":
                case "application/vnd.oma.bcast.associated-procedure-parameter+xml":
                case "application/vnd.oma.bcast.drm-trigger+xml":
                case "application/vnd.oma.bcast.imd+xml":
                case "application/vnd.oma.bcast.ltkm":
                case "application/vnd.oma.bcast.notification+xml":
                case "application/vnd.oma.bcast.provisioningtrigger":
                case "application/vnd.oma.bcast.sgboot":
                case "application/vnd.oma.bcast.sgdd+xml":
                case "application/vnd.oma.bcast.sgdu":
                case "application/vnd.oma.bcast.simple-symbol-container":
                case "application/vnd.oma.bcast.smartcard-trigger+xml":
                case "application/vnd.oma.bcast.sprov+xml":
                case "application/vnd.oma.bcast.stkm":
                case "application/vnd.oma.cab-address-book+xml":
                case "application/vnd.oma.cab-feature-handler+xml":
                case "application/vnd.oma.cab-pcc+xml":
                case "application/vnd.oma.cab-subs-invite+xml":
                case "application/vnd.oma.cab-user-prefs+xml":
                case "application/vnd.oma.dcd":
                case "application/vnd.oma.dcdc":
                case "application/vnd.oma.dd2+xml":
                case "application/vnd.oma.drm.risd+xml":
                case "application/vnd.oma.group-usage-list+xml":
                case "application/vnd.oma.lwm2m+json":
                case "application/vnd.oma.lwm2m+tlv":
                case "application/vnd.oma.pal+xml":
                case "application/vnd.oma.poc.detailed-progress-report+xml":
                case "application/vnd.oma.poc.final-report+xml":
                case "application/vnd.oma.poc.groups+xml":
                case "application/vnd.oma.poc.invocation-descriptor+xml":
                case "application/vnd.oma.poc.optimized-progress-report+xml":
                case "application/vnd.oma.push":
                case "application/vnd.oma.scidm.messages+xml":
                case "application/vnd.oma.xcap-directory+xml":
                case "application/vnd.omads-email+xml":
                case "application/vnd.omads-file+xml":
                case "application/vnd.omads-folder+xml":
                case "application/vnd.omaloc-supl-init":
                case "application/vnd.oma-scws-config":
                case "application/vnd.oma-scws-http-request":
                case "application/vnd.oma-scws-http-response":
                case "application/vnd.onepager":
                case "application/vnd.openblox.game-binary":
                case "application/vnd.openblox.game+xml":
                case "application/vnd.openeye.oeb":
                case "application/vnd.openxmlformats-officedocument.custom-properties+xml":
                case "application/vnd.openxmlformats-officedocument.customXmlProperties+xml":
                case "application/vnd.openxmlformats-officedocument.drawing+xml":
                case "application/vnd.openxmlformats-officedocument.drawingml.chart+xml":
                case "application/vnd.openxmlformats-officedocument.drawingml.chartshapes+xml":
                case "application/vnd.openxmlformats-officedocument.drawingml.diagramColors+xml":
                case "application/vnd.openxmlformats-officedocument.drawingml.diagramData+xml":
                case "application/vnd.openxmlformats-officedocument.drawingml.diagramLayout+xml":
                case "application/vnd.openxmlformats-officedocument.drawingml.diagramStyle+xml":
                case "application/vnd.openxmlformats-officedocument.extended-properties+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.commentAuthors+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.comments+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.handoutMaster+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.notesMaster+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.notesSlide+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                case "application/vnd.openxmlformats-officedocument.presentationml.presentation.main+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.presProps+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.slide":
                case "application/vnd.openxmlformats-officedocument.presentationml.slide+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.slideMaster+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.slideshow":
                case "application/vnd.openxmlformats-officedocument.presentationml.slideshow.main+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.slideUpdateInfo+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.tableStyles+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.tags+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml-template":
                case "application/vnd.openxmlformats-officedocument.presentationml.template.main+xml":
                case "application/vnd.openxmlformats-officedocument.presentationml.viewProps+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.calcChain+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.chartsheet+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.comments+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.connections+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.dialogsheet+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.externalLink+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotCacheDefinition+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotCacheRecords+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotTable+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.queryTable+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.revisionHeaders+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.revisionLog+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheetMetadata+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.table+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.tableSingleCells+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml-template":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.template.main+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.userNames+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.volatileDependencies+xml":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml":
                case "application/vnd.openxmlformats-officedocument.theme+xml":
                case "application/vnd.openxmlformats-officedocument.themeOverride+xml":
                case "application/vnd.openxmlformats-officedocument.vmlDrawing":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.comments+xml":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document.glossary+xml":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.endnotes+xml":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.fontTable+xml":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.footer+xml":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.footnotes+xml":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.numbering+xml":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.settings+xml":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml-template":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.template.main+xml":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.webSettings+xml":
                case "application/vnd.openxmlformats-package.core-properties+xml":
                case "application/vnd.openxmlformats-package.digital-signature-xmlsignature+xml":
                case "application/vnd.openxmlformats-package.relationships+xml":
                case "application/vnd.oracle.resource+json":
                case "application/vnd.orange.indata":
                case "application/vnd.osa.netdeploy":
                case "application/vnd.osgeo.mapguide.package":
                case "application/vnd.osgi.bundle":
                case "application/vnd.osgi.dp":
                case "application/vnd.osgi.subsystem":
                case "application/vnd.otps.ct-kip+xml":
                case "application/vnd.oxli.countgraph":
                case "application/vnd.pagerduty+json":
                case "application/vnd.palm":
                case "application/vnd.panoply":
                case "application/vnd.paos+xml":
                case "application/vnd.pawaafile":
                case "application/vnd.pcos":
                case "application/vnd.pg.format":
                case "application/vnd.pg.osasli":
                case "application/vnd.piaccess.application-licence":
                case "application/vnd.picsel":
                case "application/vnd.pmi.widget":
                case "application/vnd.poc.group-advertisement+xml":
                case "application/vnd.pocketlearn":
                case "application/vnd.powerbuilder6":
                case "application/vnd.powerbuilder6-s":
                case "application/vnd.powerbuilder7":
                case "application/vnd.powerbuilder75":
                case "application/vnd.powerbuilder75-s":
                case "application/vnd.powerbuilder7-s":
                case "application/vnd.preminet":
                case "application/vnd.previewsystems.box":
                case "application/vnd.proteus.magazine":
                case "application/vnd.publishare-delta-tree":
                case "application/vnd.pvi.ptid1":
                case "application/vnd.pwg-multiplexed":
                case "application/vnd.pwg-xhtml-print+xml":
                case "application/vnd.qualcomm.brew-app-res":
                case "application/vnd.quarantainenet":
                case "application/vnd.Quark.QuarkXPress":
                case "application/vnd.quobject-quoxdocument":
                case "application/vnd.radisys.moml+xml":
                case "application/vnd.radisys.msml-audit-conf+xml":
                case "application/vnd.radisys.msml-audit-conn+xml":
                case "application/vnd.radisys.msml-audit-dialog+xml":
                case "application/vnd.radisys.msml-audit-stream+xml":
                case "application/vnd.radisys.msml-audit+xml":
                case "application/vnd.radisys.msml-conf+xml":
                case "application/vnd.radisys.msml-dialog-base+xml":
                case "application/vnd.radisys.msml-dialog-fax-detect+xml":
                case "application/vnd.radisys.msml-dialog-fax-sendrecv+xml":
                case "application/vnd.radisys.msml-dialog-group+xml":
                case "application/vnd.radisys.msml-dialog-speech+xml":
                case "application/vnd.radisys.msml-dialog-transform+xml":
                case "application/vnd.radisys.msml-dialog+xml":
                case "application/vnd.radisys.msml+xml":
                case "application/vnd.rainstor.data":
                case "application/vnd.rapid":
                case "application/vnd.rar":
                case "application/vnd.realvnc.bed":
                case "application/vnd.recordare.musicxml":
                case "application/vnd.recordare.musicxml+xml":
                case "application/vnd.renlearn.rlprint":
                case "application/vnd.rig.cryptonote":
                case "application/vnd.route66.link66+xml":
                case "application/vnd.rs-274x":
                case "application/vnd.ruckus.download":
                case "application/vnd.s3sms":
                case "application/vnd.sailingtracker.track":
                case "application/vnd.sbm.cid":
                case "application/vnd.sbm.mid2":
                case "application/vnd.scribus":
                case "application/vnd.sealed.3df":
                case "application/vnd.sealed.csf":
                case "application/vnd.sealed-doc":
                case "application/vnd.sealed-eml":
                case "application/vnd.sealed-mht":
                case "application/vnd.sealed.net":
                case "application/vnd.sealed-ppt":
                case "application/vnd.sealed-tiff":
                case "application/vnd.sealed-xls":
                case "application/vnd.sealedmedia.softseal-html":
                case "application/vnd.sealedmedia.softseal-pdf":
                case "application/vnd.seemail":
                case "application/vnd-sema":
                case "application/vnd.semd":
                case "application/vnd.semf":
                case "application/vnd.shana.informed.formdata":
                case "application/vnd.shana.informed.formtemplate":
                case "application/vnd.shana.informed.interchange":
                case "application/vnd.shana.informed.package":
                case "application/vnd.SimTech-MindMapper":
                case "application/vnd.siren+json":
                case "application/vnd.smaf":
                case "application/vnd.smart.notebook":
                case "application/vnd.smart.teacher":
                case "application/vnd.software602.filler.form+xml":
                case "application/vnd.software602.filler.form-xml-zip":
                case "application/vnd.solent.sdkm+xml":
                case "application/vnd.spotfire.dxp":
                case "application/vnd.spotfire.sfs":
                case "application/vnd.sss-cod":
                case "application/vnd.sss-dtf":
                case "application/vnd.sss-ntf":
                case "application/vnd.stepmania.package":
                case "application/vnd.stepmania.stepchart":
                case "application/vnd.street-stream":
                case "application/vnd.sun.wadl+xml":
                case "application/vnd.sus-calendar":
                case "application/vnd.svd":
                case "application/vnd.swiftview-ics":
                case "application/vnd.syncml.dm.notification":
                case "application/vnd.syncml.dmddf+xml":
                case "application/vnd.syncml.dmtnds+wbxml":
                case "application/vnd.syncml.dmtnds+xml":
                case "application/vnd.syncml.dmddf+wbxml":
                case "application/vnd.syncml.dm+wbxml":
                case "application/vnd.syncml.dm+xml":
                case "application/vnd.syncml.ds.notification":
                case "application/vnd.syncml+xml":
                case "application/vnd.tao.intent-module-archive":
                case "application/vnd.tcpdump.pcap":
                case "application/vnd.tml":
                case "application/vnd.tmd.mediaflex.api+xml":
                case "application/vnd.tmobile-livetv":
                case "application/vnd.trid.tpt":
                case "application/vnd.triscape.mxs":
                case "application/vnd.trueapp":
                case "application/vnd.truedoc":
                case "application/vnd.ubisoft.webplayer":
                case "application/vnd.ufdl":
                case "application/vnd.uiq.theme":
                case "application/vnd.umajin":
                case "application/vnd.unity":
                case "application/vnd.uoml+xml":
                case "application/vnd.uplanet.alert":
                case "application/vnd.uplanet.alert-wbxml":
                case "application/vnd.uplanet.bearer-choice":
                case "application/vnd.uplanet.bearer-choice-wbxml":
                case "application/vnd.uplanet.cacheop":
                case "application/vnd.uplanet.cacheop-wbxml":
                case "application/vnd.uplanet.channel":
                case "application/vnd.uplanet.channel-wbxml":
                case "application/vnd.uplanet.list":
                case "application/vnd.uplanet.listcmd":
                case "application/vnd.uplanet.listcmd-wbxml":
                case "application/vnd.uplanet.list-wbxml":
                case "application/vnd.uri-map":
                case "application/vnd.uplanet.signal":
                case "application/vnd.valve.source.material":
                case "application/vnd.vcx":
                case "application/vnd.vd-study":
                case "application/vnd.vectorworks":
                case "application/vnd.vel+json":
                case "application/vnd.verimatrix.vcas":
                case "application/vnd.vidsoft.vidconference":
                case "application/vnd.visio":
                case "application/vnd.visionary":
                case "application/vnd.vividence.scriptfile":
                case "application/vnd.vsf":
                case "application/vnd.wap.sic":
                case "application/vnd.wap-slc":
                case "application/vnd.wap-wbxml":
                case "application/vnd-wap-wmlc":
                case "application/vnd.wap.wmlscriptc":
                case "application/vnd.webturbo":
                case "application/vnd.wfa.p2p":
                case "application/vnd.wfa.wsc":
                case "application/vnd.windows.devicepairing":
                case "application/vnd.wmc":
                case "application/vnd.wmf.bootstrap":
                case "application/vnd.wolfram.mathematica":
                case "application/vnd.wolfram.mathematica.package":
                case "application/vnd.wolfram.player":
                case "application/vnd.wordperfect":
                case "application/vnd.wqd":
                case "application/vnd.wrq-hp3000-labelled":
                case "application/vnd.wt.stf":
                case "application/vnd.wv.csp+xml":
                case "application/vnd.wv.csp+wbxml":
                case "application/vnd.wv.ssp+xml":
                case "application/vnd.xacml+json":
                case "application/vnd.xara":
                case "application/vnd.xfdl":
                case "application/vnd.xfdl.webform":
                case "application/vnd.xmi+xml":
                case "application/vnd.xmpie.cpkg":
                case "application/vnd.xmpie.dpkg":
                case "application/vnd.xmpie.plan":
                case "application/vnd.xmpie.ppkg":
                case "application/vnd.xmpie.xlim":
                case "application/vnd.yamaha.hv-dic":
                case "application/vnd.yamaha.hv-script":
                case "application/vnd.yamaha.hv-voice":
                case "application/vnd.yamaha.openscoreformat.osfpvg+xml":
                case "application/vnd.yamaha.openscoreformat":
                case "application/vnd.yamaha.remote-setup":
                case "application/vnd.yamaha.smaf-audio":
                case "application/vnd.yamaha.smaf-phrase":
                case "application/vnd.yamaha.through-ngn":
                case "application/vnd.yamaha.tunnel-udpencap":
                case "application/vnd.yaoweme":
                case "application/vnd.yellowriver-custom-menu":
                case "application/vnd.zul":
                case "application/vnd.zzazz.deck+xml":
                case "application/voicexml+xml":
                case "application/vq-rtcpxr":
                case "application/watcherinfo+xml":
                case "application/whoispp-query":
                case "application/whoispp-response":
                case "application/wita":
                case "application/wordperfect5.1":
                case "application/wsdl+xml":
                case "application/wspolicy+xml":
                case "application/x-www-form-urlencoded":
                case "application/x400-bp":
                case "application/xacml+xml":
                case "application/xcap-att+xml":
                case "application/xcap-caps+xml":
                case "application/xcap-diff+xml":
                case "application/xcap-el+xml":
                case "application/xcap-error+xml":
                case "application/xcap-ns+xml":
                case "application/xcon-conference-info-diff+xml":
                case "application/xcon-conference-info+xml":
                case "application/xenc+xml":
                case "application/xhtml+xml":
                case "application/xml":
                case "application/xml-dtd":
                case "application/xml-external-parsed-entity":
                case "application/xml-patch+xml":
                case "application/xmpp+xml":
                case "application/xop+xml":
                case "application/xv+xml":
                case "application/yang":
                case "application/yin+xml":
                case "application/zip":
                case "application/zlib":
                #endregion
                #region audio
                // audio
                case "audio/1d-interleaved-parityfec":
                case "audio/32kadpcm":
                case "audio/3gpp":
                case "audio/3gpp2":
                case "audio/ac3":
                case "audio/AMR":
                case "audio/AMR-WB":
                case "audio/amr-wb+":
                case "audio/aptx":
                case "audio/asc":
                case "audio/ATRAC-ADVANCED-LOSSLESS":
                case "audio/ATRAC-X":
                case "audio/ATRAC3":
                case "audio/basic":
                case "audio/BV16":
                case "audio/BV32":
                case "audio/clearmode":
                case "audio/CN":
                case "audio/DAT12":
                case "audio/dls":
                case "audio/dsr-es201108":
                case "audio/dsr-es202050":
                case "audio/dsr-es202211":
                case "audio/dsr-es202212":
                case "audio/DV":
                case "audio/DVI4":
                case "audio/eac3":
                case "audio/encaprtp":
                case "audio/EVRC":
                case "audio/EVRC-QCP":
                case "audio/EVRC0":
                case "audio/EVRC1":
                case "audio/EVRCB":
                case "audio/EVRCB0":
                case "audio/EVRCB1":
                case "audio/EVRCNW":
                case "audio/EVRCNW0":
                case "audio/EVRCNW1":
                case "audio/EVRCWB":
                case "audio/EVRCWB0":
                case "audio/EVRCWB1":
                case "audio/EVS":
                case "audio/example":
                case "audio/fwdred":
                case "audio/G711-0":
                case "audio/G719":
                case "audio/G7221":
                case "audio/G722":
                case "audio/G723":
                case "audio/G726-16":
                case "audio/G726-24":
                case "audio/G726-32":
                case "audio/G726-40":
                case "audio/G728":
                case "audio/G729":
                case "audio/G729D":
                case "audio/G729E":
                case "audio/GSM":
                case "audio/GSM-EFR":
                case "audio/GSM-HR-08":
                case "audio/iLBC":
                case "audio/ip-mr_v2.5":
                case "audio/L8":
                case "audio/L16":
                case "audio/L20":
                case "audio/L24":
                case "audio/LPC":
                case "audio/mobile-xmf":
                case "audio/MPA":
                case "audio/mp4":
                case "audio/MP4A-LATM":
                case "audio/mpa-robust":
                case "audio/mpeg":
                case "audio/mpeg4-generic":
                case "audio/ogg":
                case "audio/opus":
                case "audio/PCMA":
                case "audio/PCMA-WB":
                case "audio/PCMU":
                case "audio/PCMU-WB":
                case "audio/prs.sid":
                case "audio/raptorfec":
                case "audio/RED":
                case "audio/rtp-enc-aescm128":
                case "audio/rtploopback":
                case "audio/rtp-midi":
                case "audio/rtx":
                case "audio/SMV":
                case "audio/SMV0":
                case "audio/SMV-QCP":
                case "audio/sp-midi":
                case "audio/speex":
                case "audio/t140c":
                case "audio/t38":
                case "audio/telephone-event":
                case "audio/tone":
                case "audio/UEMCLIP":
                case "audio/ulpfec":
                case "audio/VDVI":
                case "audio/VMR-WB":
                case "audio/vnd.3gpp.iufp":
                case "audio/vnd.4SB":
                case "audio/vnd.audiokoz":
                case "audio/vnd.CELP":
                case "audio/vnd.cisco.nse":
                case "audio/vnd.cmles.radio-events":
                case "audio/vnd.cns.anp1":
                case "audio/vnd.cns.inf1":
                case "audio/vnd.dece.audio":
                case "audio/vnd.digital-winds":
                case "audio/vnd.dlna.adts":
                case "audio/vnd.dolby.heaac.1":
                case "audio/vnd.dolby.heaac.2":
                case "audio/vnd.dolby.mlp":
                case "audio/vnd.dolby.mps":
                case "audio/vnd.dolby.pl2":
                case "audio/vnd.dolby.pl2x":
                case "audio/vnd.dolby.pl2z":
                case "audio/vnd.dolby.pulse.1":
                case "audio/vnd.dra":
                case "audio/vnd.dts":
                case "audio/vnd.dts.hd":
                case "audio/vnd.dvb.file":
                case "audio/vnd.everad.plj":
                case "audio/vnd.hns.audio":
                case "audio/vnd.lucent.voice":
                case "audio/vnd.ms-playready.media.pya":
                case "audio/vnd.nokia.mobile-xmf":
                case "audio/vnd.nortel.vbk":
                case "audio/vnd.nuera.ecelp4800":
                case "audio/vnd.nuera.ecelp7470":
                case "audio/vnd.nuera.ecelp9600":
                case "audio/vnd.octel.sbc":
                case "audio/vnd.qcelp":
                case "audio/vnd.rhetorex.32kadpcm":
                case "audio/vnd.rip":
                case "audio/vnd.sealedmedia.softseal-mpeg":
                case "audio/vnd.vmx.cvsd":
                case "audio/vorbis":
                case "audio/vorbis-config":
                #endregion
                #region image
                // image
                case "image/cgm":
                case "image/dicom-rle":
                case "image/emf":
                case "image/example":
                case "image/fits":
                case "image/g3fax":
                case "image/jls":
                case "image/jp2":
                case "image/jpm":
                case "image/jpx":
                case "image/naplps":
                case "image/prs.btif":
                case "image/prs.pti":
                case "image/pwg-raster":
                case "image/t38":
                case "image/tiff":
                case "image/tiff-fx":
                case "image/vnd.adobe.photoshop":
                case "image/vnd.airzip.accelerator.azv":
                case "image/vnd.cns.inf2":
                case "image/vnd.dece.graphic":
                case "image/vnd-djvu":
                case "image/vnd.dwg":
                case "image/vnd.dxf":
                case "image/vnd.dvb.subtitle":
                case "image/vnd.fastbidsheet":
                case "image/vnd.fpx":
                case "image/vnd.fst":
                case "image/vnd.fujixerox.edmics-mmr":
                case "image/vnd.fujixerox.edmics-rlc":
                case "image/vnd.globalgraphics.pgb":
                case "image/vnd.microsoft.icon":
                case "image/vnd.mix":
                case "image/vnd.ms-modi":
                case "image/vnd.mozilla.apng":
                case "image/vnd.net-fpx":
                case "image/vnd.radiance":
                case "image/vnd.sealed-png":
                case "image/vnd.sealedmedia.softseal-gif":
                case "image/vnd.sealedmedia.softseal-jpg":
                case "image/vnd-svf":
                case "image/vnd.tencent.tap":
                case "image/vnd.valve.source.texture":
                case "image/vnd-wap-wbmp":
                case "image/vnd.xiff":
                case "image/vnd.zbrush.pcx":
                case "image/wmf":
                #endregion
                #region message
                // message
                case "message/CPIM":
                case "message/delivery-status":
                case "message/disposition-notification":
                case "message/example":
                case "message/feedback-report":
                case "message/global":
                case "message/global-delivery-status":
                case "message/global-disposition-notification":
                case "message/global-headers":
                case "message/http":
                case "message/imdn+xml":
                case "message/news":
                case "message/s-http":
                case "message/sip":
                case "message/sipfrag":
                case "message/tracking-status":
                case "message/vnd.si.simp":
                case "message/vnd.wfa.wsc":
                #endregion
                #region model
                // model
                case "model/example":
                case "model/gltf+json":
                case "model/iges":
                case "model/vnd.collada+xml":
                case "model/vnd-dwf":
                case "model/vnd.flatland.3dml":
                case "model/vnd.gdl":
                case "model/vnd.gs-gdl":
                case "model/vnd.gtw":
                case "model/vnd.moml+xml":
                case "model/vnd.mts":
                case "model/vnd.opengex":
                case "model/vnd.parasolid.transmit-binary":
                case "model/vnd.parasolid.transmit-text":
                case "model/vnd.rosette.annotated-data-model":
                case "model/vnd.valve.source.compiled-map":
                case "model/vnd.vtu":
                case "model/x3d-vrml":
                case "model/x3d+fastinfoset":
                case "model/x3d+xml":
                #endregion
                #region multipart
                // multipart
                case "multipart/appledouble":
                case "multipart/byteranges":
                case "multipart/encrypted":
                case "multipart/example":
                case "multipart/form-data":
                case "multipart/header-set":
                case "multipart/related":
                case "multipart/report":
                case "multipart/signed":
                case "multipart/voice-message":
                case "multipart/x-mixed-replace":
                #endregion
                #region text
                // text
                case "text/1d-interleaved-parityfec":
                case "text/cache-manifest":
                case "text/calendar":
                case "text/css":
                case "text/csv":
                case "text/csv-schema":
                case "text/directory":
                case "text/dns":
                case "text/ecmascript":
                case "text/encaprtp":
                case "text/example":
                case "text/fwdred":
                case "text/grammar-ref-list":
                case "text/html":
                case "text/javascript":
                case "text/jcr-cnd":
                case "text/markdown":
                case "text/mizar":
                case "text/n3":
                case "text/parameters":
                case "text/provenance-notation":
                case "text/prs.fallenstein.rst":
                case "text/prs.lines.tag":
                case "text/prs.prop.logic":
                case "text/raptorfec":
                case "text/RED":
                case "text/rfc822-headers":
                case "text/rtf":
                case "text/rtp-enc-aescm128":
                case "text/rtploopback":
                case "text/rtx":
                case "text/SGML":
                case "text/t140":
                case "text/tab-separated-values":
                case "text/troff":
                case "text/turtle":
                case "text/ulpfec":
                case "text/vcard":
                case "text/vnd-a":
                case "text/vnd.abc":
                case "text/vnd-curl":
                case "text/vnd.debian.copyright":
                case "text/vnd.DMClientScript":
                case "text/vnd.dvb.subtitle":
                case "text/vnd.esmertec.theme-descriptor":
                case "text/vnd.fly":
                case "text/vnd.fmi.flexstor":
                case "text/vnd.graphviz":
                case "text/vnd.in3d.3dml":
                case "text/vnd.in3d.spot":
                case "text/vnd.IPTC.NewsML":
                case "text/vnd.IPTC.NITF":
                case "text/vnd.latex-z":
                case "text/vnd.motorola.reflex":
                case "text/vnd.ms-mediapackage":
                case "text/vnd.net2phone.commcenter.command":
                case "text/vnd.radisys.msml-basic-layout":
                case "text/vnd.si.uricatalogue":
                case "text/vnd.sun.j2me.app-descriptor":
                case "text/vnd.trolltech.linguist":
                case "text/vnd.wap.si":
                case "text/vnd.wap.sl":
                case "text/vnd.wap-wml":
                case "text/vnd.wap.wmlscript":
                case "text/xml":
                case "text/xml-external-parsed-entity":
                #endregion
                #region video
                // video
                case "video/1d-interleaved-parityfec":
                case "video/3gpp":
                case "video/3gpp2":
                case "video/3gpp-tt":
                case "video/BMPEG":
                case "video/BT656":
                case "video/CelB":
                case "video/DV":
                case "video/encaprtp":
                case "video/example":
                case "video/H261":
                case "video/H263":
                case "video/H263-1998":
                case "video/H263-2000":
                case "video/H264":
                case "video/H264-RCDO":
                case "video/H264-SVC":
                case "video/H265":
                case "video/iso.segment":
                case "video/JPEG":
                case "video/jpeg2000":
                case "video/mj2":
                case "video/MP1S":
                case "video/MP2P":
                case "video/MP2T":
                case "video/MP4V-ES":
                case "video/MPV":
                case "video/mpeg4-generic":
                case "video/nv":
                case "video/ogg":
                case "video/pointer":
                case "video/quicktime":
                case "video/raptorfec":
                case "video/rtp-enc-aescm128":
                case "video/rtploopback":
                case "video/rtx":
                case "video/SMPTE292M":
                case "video/ulpfec":
                case "video/vc1":
                case "video/vnd.CCTV":
                case "video/vnd.dece.hd":
                case "video/vnd.dece.mobile":
                case "video/vnd.dece-mp4":
                case "video/vnd.dece.pd":
                case "video/vnd.dece.sd":
                case "video/vnd.dece.video":
                case "video/vnd.directv-mpeg":
                case "video/vnd.directv.mpeg-tts":
                case "video/vnd.dlna.mpeg-tts":
                case "video/vnd.dvb.file":
                case "video/vnd.fvt":
                case "video/vnd.hns.video":
                case "video/vnd.iptvforum.1dparityfec-1010":
                case "video/vnd.iptvforum.1dparityfec-2005":
                case "video/vnd.iptvforum.2dparityfec-1010":
                case "video/vnd.iptvforum.2dparityfec-2005":
                case "video/vnd.iptvforum.ttsavc":
                case "video/vnd.iptvforum.ttsmpeg2":
                case "video/vnd.motorola.video":
                case "video/vnd.motorola.videop":
                case "video/vnd-mpegurl":
                case "video/vnd.ms-playready.media.pyv":
                case "video/vnd.nokia.interleaved-multimedia":
                case "video/vnd.nokia.videovoip":
                case "video/vnd.objectvideo":
                case "video/vnd.radgamettools.bink":
                case "video/vnd.radgamettools.smacker":
                case "video/vnd.sealed.mpeg1":
                case "video/vnd.sealed.mpeg4":
                case "video/vnd.sealed-swf":
                case "video/vnd.sealedmedia.softseal-mov":
                case "video/vnd.uvvu-mp4":
                case "video/vnd-vivo":
                case "video/VP8":
                    #endregion

                    DumpSource = null;
                    TargetUrl = string.Empty;
                    RawMessage = $"Handling for {dump.MediaType} not implemented";
                    break;

                #endregion
                default:
                    DumpSource = null;
                    TargetUrl = string.Empty;

                    RawMessage = "Unknown message type";
                    break;
            }


        }

        async void _onTestButton()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var str = await _filePickerService.GetFileStringAsync();
                var entity = new DumpWireEntity
                {
                    Id = Guid.NewGuid(),
                    EncodedData = str,
                    MediaType = "something",
                    RawData = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
                };
                await _sendDumpService.SendDump(entity);
            });
        }

        public override void OnInitialise()
        {
            _test();
            base.OnInitialise();
        }

        async void _test()
        {
            var resutl = await _testService.TestToken(_tokenService.Token);
        }

        public string MainText
        {
            get { return _mainText; }
            set
            {
                _mainText = value; 
                OnPropertyChanged();
            }
        }

        private string rawMessage;

        public string RawMessage
        {
            get { return rawMessage; }
            set { rawMessage = value; OnPropertyChanged(); }
        }

        private string targetUrl;

        public string TargetUrl
        {
            get { return targetUrl; }
            set { targetUrl = value; OnPropertyChanged(); OnPropertyChanged("ShowWebView"); }
        }

        public bool ShowWebView
        {
            get { return !String.IsNullOrWhiteSpace(TargetUrl); }
        }

        private bool showMediaPlayer;

        public bool ShowMediaplayer
        {
            get { return showMediaPlayer; }
            set { showMediaPlayer = value; OnPropertyChanged(); }
        }


    }
}
